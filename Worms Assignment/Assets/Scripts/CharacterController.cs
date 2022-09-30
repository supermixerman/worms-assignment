using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] GameObject activePlayer;
    PlayerInput activePlayerInput;
    public Player activePlayerCode;
    [SerializeField] Rigidbody rb;
    //[SerializeField] GameObject camHolder;
    [SerializeField] float speed = 10 , force = 5, rotationSpeed = 5, lookSpeed = 10;
    Vector2 move, look;
    PlayerInputs playerInput;
    bool isAiming = false;
    public bool turnOver = false;
    AudioManager audioManager;

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
        //playerInput = new PlayerInputs();
        //playerInput.Player.Enable();
        //playerInput.Player.Jump.performed += Jump;
        //playerInput.Player.Move.performed += Move;
    }

    //Enable all important inputs for players.
    private void OnEnable() { 
        playerInput = new PlayerInputs();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Aim.performed += Aim;
        playerInput.Player.Fire.performed += Shoot;
        playerInput.Player.Confirm.performed += StartTurn;
    }

    private void OnDisable() {
        playerInput.Player.Disable();
    }

    //Makes sure the first player is being controlled on start.
    private void Start() {
        SetActivePlayer(activePlayer);
    }

    private void FixedUpdate() {
        if (turnOver) { //To make sure players can't move while moving to next turn.
            if(isAiming){ //Stops a bug where the camera freezes on one players aim camera
                activePlayerCode.DeactivateAimCamera();
                isAiming = false;
            }
            return;
        }
        Move();
        if(isAiming){
            RotateWeapon(); //Rotates the weapon for aiming if the player is in aim mode.
        }
    }

    public void Move(){
        move = playerInput.Player.Move.ReadValue<Vector2>(); //Takes the input value from the Move input. Vector inputs are normalized.
        Vector3 direction = new Vector3(move.x, 0, move.y); //Set the input values into the wished directions in a Vector3.
        
        //Players rotate left or right when pressing/tilting on the side of the input.
        if (direction.x != 0){ 
            //audioManager.PlaySound("move");
            RotatePlayer(direction.x, rotationSpeed);
        }

        //Players move forward or backward when pressing/tilting the up and down inputs.
        if (direction.z != 0 && activePlayerCode.IsGrounded()){
            audioManager.PlaySound("move");
            rb.AddForce(rb.transform.forward*direction.z*speed, ForceMode.Force);
             //Debug.Log("Moving");
        }
        //Let's the players move in the air with a slower speed.
        else if (direction.z != 0 && !activePlayerCode.IsGrounded()){ 
            rb.AddForce(rb.transform.forward*direction.z*force, ForceMode.Force);
        }
        //Stops players movement to avoid sliding when not moving
        else{
            rb.AddForce(Vector3.zero, ForceMode.Force);
        }
    }

    //Players jump function
    public void Jump(InputAction.CallbackContext context){
        //Player can only jump while on the ground.
        if (context.performed && activePlayerCode.IsGrounded() && !turnOver){
            Debug.Log("Jumped");
            rb.velocity = new Vector3(0, force, 0); //I use velocity to make the jump more instant.
            audioManager.PlaySound("jump");
        }
    }

    //Function for players aiming the gun in the game.
    public void Aim(InputAction.CallbackContext context){
        if(turnOver) return; //Stops players to run this function while waiting for the next turn.
        audioManager.PlaySound("aim");
        if (!isAiming){
            activePlayerCode.ActivateAimCamera();
            isAiming = true;
        }
        else{
            activePlayerCode.DeactivateAimCamera();
            isAiming = false;
        }
    }

    public void Shoot(InputAction.CallbackContext context){
        if(!isAiming||turnOver){
            return;
        }
        activePlayerCode.GetWeaponFire();
        activePlayerCode.DeactivateAimCamera();
        audioManager.PlaySound("fire");
        isAiming = false;
        turnOver = true;
        StartCoroutine(GameManager.gameManager.WaitForNextTurn(3f));
        Debug.Log("Fire");
    }

    public void RotateWeapon(){
        look = playerInput.Player.Look.ReadValue<Vector2>();
        Vector3 lookDirection = new Vector3(look.x, 0, look.y);
        //Debug.Log("Rotating weapon"+lookDirection);
        if (lookDirection.z != 0){
            activePlayerCode.GetWeapon().GetComponent<Transform>().transform.Rotate(-lookDirection.z*lookSpeed*Time.fixedDeltaTime, 0, 0, Space.Self);
        }
        if (lookDirection.x != 0){
            RotatePlayer(lookDirection.x, lookSpeed);
        }
    }

    public void StartTurn(InputAction.CallbackContext context){
        if (!turnOver) return;
        GameManager.gameManager.ResumeTurn();
        turnOver = false;
        Debug.Log("Turn Start");
    }

    public void SetActivePlayer(GameObject player){
        activePlayer = player;
        rb = activePlayer.GetComponent<Rigidbody>();
        activePlayerInput = activePlayer.GetComponent<PlayerInput>();
        activePlayerCode = activePlayer.GetComponent<Player>();
    }

    public void RotatePlayer(float direction, float rotSpeed){
        Quaternion rotation = Quaternion.Euler(0, direction*rotSpeed*Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation*rotation);
        //Debug.Log("Rotating");
    }    
}

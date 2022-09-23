using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] GameObject activePlayer;
    PlayerInput activePlayerInput;
    Player activePlayerCode;
    [SerializeField] Rigidbody rb;
    Vector3 eulerAngleVelocity;
    //[SerializeField] GameObject camHolder;
    [SerializeField] float speed = 10 , force = 5, rotationSpeed = 5, lookSpeed = 10;
    Vector2 move, look;
    PlayerInputs playerInput;
    bool isAiming = false;

    private void Awake() {
        eulerAngleVelocity = new Vector3(0, 100, 0);
        //playerInput = new PlayerInputs();
        //playerInput.Player.Enable();
        //playerInput.Player.Jump.performed += Jump;
        //playerInput.Player.Move.performed += Move;
    }

    private void OnEnable() {
        playerInput = new PlayerInputs();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Aim.performed += Aim;
        playerInput.Player.Fire.performed += Shoot;
        //playerInput.Player.Aim.canceled += Aim;
    }

    private void OnDisable() {
        playerInput.Player.Disable();
    }

    private void Start() {
        SetActivePlayer(activePlayer);
    }

    private void FixedUpdate() {
        Move();
        RotateWeapon();
    }

    public void Move(){
        move = playerInput.Player.Move.ReadValue<Vector2>();
        //Debug.Log(move);
        Vector3 direction = new Vector3(move.x, 0, move.y);
        if (direction.x != 0){
            RotatePlayer(direction.x, rotationSpeed);
        }
        if (direction.z != 0 && activePlayerCode.IsGrounded()){
            rb.AddForce(rb.transform.forward*direction.z*speed, ForceMode.Force);
             Debug.Log("Moving");
        }
        else if (direction.z != 0 && !activePlayerCode.IsGrounded()){ //Let's the player move in the air with a slower speed.
            rb.AddForce(rb.transform.forward*direction.z*force, ForceMode.Force);
        }
        else{
            rb.AddForce(Vector3.zero, ForceMode.Force); //Stops players movement to avoid sliding when not moving
        }
    }
    public void Jump(InputAction.CallbackContext context){
        if (context.performed && activePlayerCode.IsGrounded()){
            Debug.Log("Jumped");
            rb.velocity = new Vector3(0, force, 0);
        }
    }

    public void Aim(InputAction.CallbackContext context){
        if (!isAiming){
            activePlayerCode.ActivateAimCamera();
            isAiming = true;
        }
        else{
            activePlayerCode.DeactivateAimCamera();
            isAiming = false;
        }
        /*if (context.canceled){

        }*/
    }

    public void Shoot(InputAction.CallbackContext context){
        if(!isAiming){
            return;
        }
        activePlayerCode.GetWeaponFire();
        Debug.Log("Fire");
    }

    public void RotateWeapon(){
        look = playerInput.Player.Look.ReadValue<Vector2>();
        Vector3 lookDirection = new Vector3(look.x, 0, look.y);
        //Debug.Log("Rotating weapon"+lookDirection);
        if (lookDirection.z != 0){
            activePlayerCode.GetWeapon().GetComponent<Transform>().transform.Rotate(-lookDirection.z*lookSpeed*Time.fixedDeltaTime, 0, 0, Space.Self);
            //Quaternion weaponRotation = Quaternion.Euler(lookDirection.x*rotationSpeed, 0, 0);
            //activePlayerCode.GetWeapon().GetComponent<Transform>().transform.rotation = Quaternion.Slerp(transform.rotation, weaponRotation, Time.fixedDeltaTime);
        }
        if (lookDirection.x != 0){
            RotatePlayer(lookDirection.x, lookSpeed);
        }
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
            Debug.Log("Rotating");
    }
    
    /*PlayerInputs playerInput;
    Vector2 moveInput;
    Vector3 movement;
    bool movePressed;
    // Start is called before the first frame update
    private void Awake() {
        playerInput = new PlayerInputs();
        playerInput.Player.Move.started += context => {Debug.Log(context.ReadValue<Vector2>());};
    }

    void MovementInput (InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();
        movement.x = moveInput.x;
        movement.z = moveInput.y;
        movePressed = moveInput.x != 0 || moveInput.y != 0;
    }*/

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] GameObject activePlayer;
    PlayerInput activePlayerInput;
    [SerializeField] Rigidbody rb;
    Vector3 eulerAngleVelocity;
    //[SerializeField] GameObject camHolder;
    [SerializeField] float speed = 10 , force = 5, rotationSpeed = 5;
    Vector2 move, look;
    PlayerInputs playerInput;

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
    }

    private void OnDisable() {
        playerInput.Player.Disable();
    }

    private void Start() {
        SetActivePlayer(activePlayer);
    }

    private void FixedUpdate() {
        Move();
    }

    public void Move(){
        move = playerInput.Player.Move.ReadValue<Vector2>();
        //Vector3 f = transform.forward * move.y;
        //Vector3 r = transform.right * move.x;
        //Vector3 combined = (f+r).normalized;
        Debug.Log(move);
        Vector3 direction = new Vector3(move.x, 0, move.y);
        if (direction.x != 0){
            Quaternion rotation = Quaternion.Euler(0, direction.x*rotationSpeed*Time.fixedDeltaTime, 0);
            rb.MoveRotation(rb.rotation*rotation);
            //rb.rotation = Quaternion.RotateTowards(rb.rotation, rotation, rotationSpeed*Time.fixedDeltaTime);
            //activePlayer.transform.forward = direction;
             Debug.Log("Rotating");
        }
        if (direction.z != 0){
            rb.AddForce(rb.transform.forward*direction.z*speed, ForceMode.Force);
             Debug.Log("Moving");
        }
    }
    public void Jump(InputAction.CallbackContext context){
        if (context.performed){
            Debug.Log("Jumped");
            rb.AddForce(Vector3.up*force,ForceMode.Impulse);
        }
    }

    public void SetActivePlayer(GameObject player){
        activePlayer = player;
        rb = activePlayer.GetComponent<Rigidbody>();
        activePlayerInput = activePlayer.GetComponent<PlayerInput>();
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

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private PlayerAnimationController animationController;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float gravity = -9.8f;

    private float currentSpeed;
    private Vector3 velocity;
    private bool isGrounded;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<PlayerAnimationController>(); 
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);

        
        currentSpeed = Keyboard.current.leftShiftKey.isPressed ? runSpeed : walkSpeed;
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void ProcessMove(Vector2 input)
    {
        moveInput = input;
    }

}

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
    [SerializeField] private float jumpForce = 5f; // Jump force is handled here

    private float currentSpeed;
    private Vector3 velocity;
    private bool isGrounded;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<PlayerAnimationController>(); // Get animation controller
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // Prevent constant falling

        // Movement logic
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);

        // Apply movement speed
        currentSpeed = Keyboard.current.leftShiftKey.isPressed ? runSpeed : walkSpeed;
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void ProcessMove(Vector2 input)
    {
        moveInput = input;
    }

}

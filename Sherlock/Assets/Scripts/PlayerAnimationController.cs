using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    [SerializeField] private float acceleration = 3f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private float gravity = -9.8f;

    private float velocity = 0f;
    private Vector3 playerVelocity;
    private bool isGrounded;

    private static readonly int VelocityHash = Animator.StringToHash("Velocity");
    private static readonly int PickUpHash = Animator.StringToHash("PickUp");
    private static readonly int PickUpDoneHash = Animator.StringToHash("PickUpDone");
    private static readonly int DropHash = Animator.StringToHash("Drop");
    private static readonly int DropDoneHash = Animator.StringToHash("DropDone");

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        onFoot.PickUp.performed += ctx => PlayPickAnimation();
        onFoot.Drop.performed += ctx => PlayDropAnimation();
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        Vector2 moveInput = onFoot.Movement.ReadValue<Vector2>();
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        bool isMovingForward = moveInput.y > 0;
        bool isRunning = onFoot.Run.IsPressed() && isMovingForward;

        float targetVelocity = isMoving ? (isRunning ? 1f : 0.5f) : 0f;

        if (isMoving)
        {
            velocity = Mathf.MoveTowards(velocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            velocity = Mathf.MoveTowards(velocity, 0f, deceleration * Time.deltaTime);
        }

        animator.SetFloat(VelocityHash, velocity);

        // Apply gravity
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void PlayPickAnimation()
    {
        animator.SetTrigger(PickUpHash);
        StartCoroutine(ResetToBlendTree(PickUpDoneHash, 1f));
    }

    public void PlayDropAnimation()
    {
        animator.SetTrigger(DropHash);
        StartCoroutine(ResetToBlendTree(DropDoneHash, 1f));
    }

    private IEnumerator ResetToBlendTree(int triggerHash, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(triggerHash);
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}

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
    private bool isAnimationPlaying = false;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private float zoomOutDuration = 1f;
    [SerializeField] private Vector3 thirdPersonOffset = new Vector3(0, 2.5f, -5);
    [SerializeField] private float cameraFollowSpeed = 5f;
    [SerializeField] private float cameraAngle = 15f;

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

        if (thirdPersonCamera != null)
        {
            thirdPersonCamera.SetActive(false);
        }
    }

    void Update()
    {
        if (isAnimationPlaying) return;

        HandleMovement();
        HandleCameraFollow();
    }

    private void HandleMovement()
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

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void HandleCameraFollow()
    {
        if (thirdPersonCamera == null || !thirdPersonCamera.activeSelf) return;

        Vector3 desiredPosition = transform.position + thirdPersonOffset;
        thirdPersonCamera.transform.position = Vector3.Lerp(thirdPersonCamera.transform.position, desiredPosition, cameraFollowSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(cameraAngle, transform.eulerAngles.y, 0);
        thirdPersonCamera.transform.rotation = Quaternion.Slerp(thirdPersonCamera.transform.rotation, targetRotation, cameraFollowSpeed * Time.deltaTime);
    }

    public void PlayPickAnimation()
    {
        if (isAnimationPlaying) return;
        StartCoroutine(HandleAnimation(PickUpHash, PickUpDoneHash));
    }

    public void PlayDropAnimation()
    {
        if (isAnimationPlaying) return;
        StartCoroutine(HandleAnimation(DropHash, DropDoneHash));
    }

    private IEnumerator HandleAnimation(int startTrigger, int endTrigger)
    {
        isAnimationPlaying = true;
        LockInput();
        animator.SetTrigger(startTrigger);

        StartCoroutine(SwitchToThirdPersonCamera(zoomOutDuration));

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        animator.SetTrigger(endTrigger);

        SwitchToMainCamera();

        UnlockInput();
        isAnimationPlaying = false;
    }

    private IEnumerator SwitchToThirdPersonCamera(float duration)
    {
        if (thirdPersonCamera == null || mainCamera == null)
        {
            Debug.LogWarning("Camera references are not set!");
            yield break;
        }

        thirdPersonCamera.SetActive(true);
        mainCamera.SetActive(false);

        Vector3 initialOffset = thirdPersonCamera.transform.position - transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            thirdPersonCamera.transform.position = Vector3.Lerp(transform.position + initialOffset, transform.position + thirdPersonOffset, t);
            thirdPersonCamera.transform.LookAt(transform);
            yield return null;
        }

        thirdPersonCamera.transform.position = transform.position + thirdPersonOffset;
        thirdPersonCamera.transform.LookAt(transform);
    }

    private void SwitchToMainCamera()
    {
        if (thirdPersonCamera != null && mainCamera != null)
        {
            thirdPersonCamera.SetActive(false);
            mainCamera.SetActive(true);
        }
    }

    private void LockInput()
    {
        playerInput.Disable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockInput()
    {
        playerInput.Enable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
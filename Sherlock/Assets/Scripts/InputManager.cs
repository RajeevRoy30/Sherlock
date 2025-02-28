using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    public PlayerMove playerMove;
    public PlayerLook playerLook;
    public PlayerAnimationController playerAnimation;

    private void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        // Movement & Look
        onFoot.Movement.performed += ctx => playerMove.ProcessMove(ctx.ReadValue<Vector2>());
        onFoot.Movement.canceled += ctx => playerMove.ProcessMove(Vector2.zero);
        onFoot.Look.performed += ctx => playerLook.ProcessLook(ctx.ReadValue<Vector2>());

        // ✅ Calls the public animation functions
        onFoot.PickUp.performed += ctx => playerAnimation.PlayPickAnimation();
        onFoot.Drop.performed += ctx => playerAnimation.PlayDropAnimation();
    }

    private void OnEnable() => onFoot.Enable();
    private void OnDisable() => onFoot.Disable();
}

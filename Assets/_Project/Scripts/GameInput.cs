using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event Action OnJumpPressed;
    public event Action OnCrouchPressed;
    public event Action OnSprintPressed;
    public event Action OnSprintReleased;

    private PlayerInputSet inputSet;
    private PlayerInputSet.PlayerActions player;

    private void Awake()
    {
        inputSet = new PlayerInputSet();
        player = inputSet.Player;
    }

    private void OnEnable()
    {
        inputSet.Enable();
        player.Jump.performed += OnJumpPerformed;
        player.Crouch.performed += OnCrouchPerformed;
        player.Sprint.started += OnSprintStarted;
        player.Sprint.canceled += OnSprintCanceled;
    }

    private void OnDisable()
    {
        inputSet.Disable();
        player.Jump.performed -= OnJumpPerformed;
        player.Crouch.performed -= OnCrouchPerformed;
        player.Sprint.started -= OnSprintStarted;
        player.Sprint.canceled -= OnSprintCanceled;
    }

    public Vector2 GetMoveInputNormalized() => player.Movement.ReadValue<Vector2>();

    public Vector2 GetLookInput() => player.Look.ReadValue<Vector2>();

    private void OnJumpPerformed(InputAction.CallbackContext obj) => OnJumpPressed?.Invoke();
    private void OnCrouchPerformed(InputAction.CallbackContext obj) => OnCrouchPressed?.Invoke();
    private void OnSprintStarted(InputAction.CallbackContext obj) => OnSprintPressed?.Invoke();
    private void OnSprintCanceled(InputAction.CallbackContext obj) => OnSprintReleased?.Invoke();
}

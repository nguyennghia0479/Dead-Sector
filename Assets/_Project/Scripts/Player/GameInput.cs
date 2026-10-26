using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event Action OnJumpPressed;
    public event Action OnCrouchPressed;
    public event Action OnSprintPressed;
    public event Action OnSprintReleased;
    public event Action OnShootPressed;
    public event Action OnShootReleased;
    public event Action OnReloadPressed;

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
        player.Sprint.performed += OnSprintPerformed;
        player.Sprint.canceled += OnSprintCanceled;
        player.Shoot.performed += OnShootPerformed;
        player.Shoot.canceled += OnShootCanceled;
        player.Reload.performed += OnReloadPerformed;
    }

    private void OnDisable()
    {
        inputSet.Disable();
        player.Jump.performed -= OnJumpPerformed;
        player.Crouch.performed -= OnCrouchPerformed;
        player.Sprint.performed -= OnSprintPerformed;
        player.Sprint.canceled -= OnSprintCanceled;
        player.Shoot.performed -= OnShootPerformed;
        player.Shoot.canceled -= OnShootCanceled;
        player.Reload.performed -= OnReloadPerformed;
    }

    public Vector2 GetMoveInputNormalized() => player.Movement.ReadValue<Vector2>();

    public Vector2 GetLookInput() => player.Look.ReadValue<Vector2>();

    private void OnJumpPerformed(InputAction.CallbackContext obj) => OnJumpPressed?.Invoke();
    private void OnCrouchPerformed(InputAction.CallbackContext obj) => OnCrouchPressed?.Invoke();
    private void OnSprintPerformed(InputAction.CallbackContext obj) => OnSprintPressed?.Invoke();
    private void OnSprintCanceled(InputAction.CallbackContext obj) => OnSprintReleased?.Invoke();
    private void OnShootPerformed(InputAction.CallbackContext obj) => OnShootPressed?.Invoke();
    private void OnShootCanceled(InputAction.CallbackContext obj) => OnShootReleased?.Invoke();
    private void OnReloadPerformed(InputAction.CallbackContext obj) => OnReloadPressed?.Invoke();
}

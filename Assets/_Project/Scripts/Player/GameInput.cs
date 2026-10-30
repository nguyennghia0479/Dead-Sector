using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class GameInput : MonoBehaviour
{
    public event Action OnJumpPressed;
    public event Action OnCrouchPressed;
    public event Action OnSprintPressed;
    public event Action OnSprintReleased;
    public event Action OnShootPressed;
    public event Action OnShootReleased;
    public event Action OnReloadPressed;
    public event Action OnAimPressed;
    public event Action<int> OnSlotSelected;
    public event Action OnInteractPressed;
    public event Action OnInteractReleased;

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
        player.Aim.performed += OnAimPerformed;
        player.SwitchGun.performed += OnSwitchPerformend;
        player.Interaction.performed += OnInteractStarted;
        player.Interaction.canceled += OnInteractCanceled;
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
        player.Aim.performed -= OnAimPerformed;
        player.SwitchGun.performed -= OnSwitchPerformend;
        player.Interaction.performed -= OnInteractStarted;
        player.Interaction.canceled -= OnInteractCanceled;
    }

    public Vector2 GetMoveInputNormalized() => player.Movement.ReadValue<Vector2>();

    public Vector2 GetMouseInput() => player.Look.ReadValue<Vector2>();

    private void OnJumpPerformed(InputAction.CallbackContext context) => OnJumpPressed?.Invoke();
    private void OnCrouchPerformed(InputAction.CallbackContext context) => OnCrouchPressed?.Invoke();
    private void OnSprintPerformed(InputAction.CallbackContext context) => OnSprintPressed?.Invoke();
    private void OnSprintCanceled(InputAction.CallbackContext context) => OnSprintReleased?.Invoke();
    private void OnShootPerformed(InputAction.CallbackContext context) => OnShootPressed?.Invoke();
    private void OnShootCanceled(InputAction.CallbackContext context) => OnShootReleased?.Invoke();
    private void OnReloadPerformed(InputAction.CallbackContext context) => OnReloadPressed?.Invoke();
    private void OnAimPerformed(InputAction.CallbackContext context) => OnAimPressed?.Invoke();
    private void OnInteractStarted(InputAction.CallbackContext context) => OnInteractPressed?.Invoke();
    private void OnInteractCanceled(InputAction.CallbackContext context) => OnInteractReleased?.Invoke();

    private void OnSwitchPerformend(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (context.control is not KeyControl key) return;

        switch (key.keyCode)
        {
            case Key.Digit1:
                OnSlotSelected?.Invoke(0);
                break;

            case Key.Digit2:
                OnSlotSelected?.Invoke(1);
                break;
        }
    }

    public string InteractionBindingDisplay => player.Interaction.GetBindingDisplayString();
}

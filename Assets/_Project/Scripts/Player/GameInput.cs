using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum Binding
{
    MoveUp, MoveDown, MoveLeft, MoveRight, Sprint, Jump, Crouch, Shoot, Reload, Aim, Interact, ThrowGrenade, ThrowMolotov, PauseGame, PrimaryGun, SecondaryGun
}

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
    public event Action<ThrowableType> OnThrown;

    private PlayerInputSet inputSet;
    private PlayerInputSet.PlayerActions player;
    private PlayerInputSet.UIActions ui;

    private void Awake()
    {
        inputSet = new PlayerInputSet();
        player = inputSet.Player;
        ui = inputSet.UI;
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
        player.Interaction.performed += OnInteractPerformed;
        player.Interaction.canceled += OnInteractCanceled;
        player.Throw.performed += OnThrowPerformed;
        player.PauseGame.performed += OnPauseGamePerformed;

        ui.ResumeGame.performed += OnResumeGamePerformed;
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
        player.Interaction.performed -= OnInteractPerformed;
        player.Interaction.canceled -= OnInteractCanceled;
        player.Throw.performed -= OnThrowPerformed;
        player.PauseGame.performed -= OnPauseGamePerformed;

        ui.ResumeGame.performed += OnResumeGamePerformed;
    }

    public void EnablePlayerInput() => inputSet.Player.Enable();
    public void DisablePlayerInput() => inputSet.Player.Disable();
    public void EnableUIInput() => inputSet.UI.Enable();
    public void DisableUIInput() => inputSet.UI.Disable();

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
    private void OnInteractPerformed(InputAction.CallbackContext context) => OnInteractPressed?.Invoke();
    private void OnInteractCanceled(InputAction.CallbackContext context) => OnInteractReleased?.Invoke();
    private void OnPauseGamePerformed(InputAction.CallbackContext context) => GameEvents.RaisePauseGameButtonPressed(true);
    private void OnResumeGamePerformed(InputAction.CallbackContext context) => GameEvents.RaisePauseGameButtonPressed(false);

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

    private void OnThrowPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (context.control is not KeyControl key) return;

        switch (key.keyCode)
        {
            case Key.G:
                OnThrown?.Invoke(ThrowableType.Grenade);
                break;
            case Key.Q:
                OnThrown?.Invoke(ThrowableType.Molotov);
                break;
        }
    }

    public string GetBindingDisplay(Binding binding)
    {
        return binding switch
        {
            Binding.MoveUp => player.Movement.bindings[1].ToDisplayString(),
            Binding.MoveDown => player.Movement.bindings[2].ToDisplayString(),
            Binding.MoveLeft => player.Movement.bindings[3].ToDisplayString(),
            Binding.MoveRight => player.Movement.bindings[4].ToDisplayString(),
            Binding.Sprint => player.Sprint.bindings[0].ToDisplayString(),
            Binding.Jump => player.Jump.bindings[0].ToDisplayString(),
            Binding.Crouch => player.Crouch.bindings[0].ToDisplayString(),
            Binding.Shoot => player.Shoot.bindings[0].ToDisplayString(),
            Binding.Reload => player.Reload.bindings[0].ToDisplayString(),
            Binding.Aim => player.Aim.bindings[0].ToDisplayString(),
            Binding.Interact => player.Interaction.bindings[0].ToDisplayString(),
            Binding.ThrowGrenade => player.Throw.bindings[0].ToDisplayString(),
            Binding.ThrowMolotov => player.Throw.bindings[1].ToDisplayString(),
            Binding.PauseGame => player.PauseGame.bindings[0].ToDisplayString(),
            Binding.PrimaryGun => player.SwitchGun.bindings[0].ToDisplayString(),
            Binding.SecondaryGun => player.SwitchGun.bindings[1].ToDisplayString(),
            _ => string.Empty,
        };
    }
}

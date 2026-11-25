using TMPro;
using UnityEngine;

public class GameInputsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moveUpBtnText;
    [SerializeField] private TMP_Text moveDownBtnText;
    [SerializeField] private TMP_Text moveLeftBtnText;
    [SerializeField] private TMP_Text moveRightBtnText;
    [SerializeField] private TMP_Text sprintBtnText;
    [SerializeField] private TMP_Text jumpBtnText;
    [SerializeField] private TMP_Text crouchBtnText;
    [SerializeField] private TMP_Text shootBtnText;
    [SerializeField] private TMP_Text reloadBtnText;
    [SerializeField] private TMP_Text aimBtnText;
    [SerializeField] private TMP_Text primaryGunBtnText;
    [SerializeField] private TMP_Text secondaryGunBtnText;
    [SerializeField] private TMP_Text throwGrenadeBtnText;
    [SerializeField] private TMP_Text throwMolotovBtnText;
    [SerializeField] private TMP_Text interactBtnText;
    [SerializeField] private TMP_Text pauseGameBtnText;

    public void Initialized(GameInput gameInput)
    {
        if (gameInput == null) return;

        moveUpBtnText.text = gameInput.GetBindingDisplay(Binding.MoveUp);
        moveDownBtnText.text = gameInput.GetBindingDisplay(Binding.MoveDown);
        moveLeftBtnText.text = gameInput.GetBindingDisplay (Binding.MoveLeft);
        moveRightBtnText.text = gameInput.GetBindingDisplay(Binding.MoveRight);
        sprintBtnText.text = gameInput.GetBindingDisplay(Binding.Sprint);
        jumpBtnText.text = gameInput.GetBindingDisplay(Binding.Jump);
        crouchBtnText.text = gameInput.GetBindingDisplay(Binding.Crouch);
        shootBtnText.text = gameInput.GetBindingDisplay(Binding.Shoot);
        reloadBtnText.text = gameInput.GetBindingDisplay(Binding.Reload);
        aimBtnText.text = gameInput.GetBindingDisplay(Binding.Aim);
        primaryGunBtnText.text = gameInput.GetBindingDisplay(Binding.PrimaryGun);
        secondaryGunBtnText.text = gameInput.GetBindingDisplay(Binding.SecondaryGun);
        throwGrenadeBtnText.text = gameInput.GetBindingDisplay(Binding.ThrowGrenade);
        throwMolotovBtnText.text = gameInput.GetBindingDisplay(Binding.ThrowMolotov);
        interactBtnText.text = gameInput.GetBindingDisplay(Binding.Interact);
        pauseGameBtnText.text = gameInput.GetBindingDisplay(Binding.PauseGame);
    }
}

using UnityEngine;

[System.Serializable]
public class GunSway
{
    [Header("Mouse Sway Settings")]
    [SerializeField] private float swayClamp = .05f;
    [SerializeField] private float smoothTime = 5f;
    [SerializeField] private float swayMultiplier = .1f;

    [Header("Idle Sway")]
    [SerializeField] private float breathingAmplitude = .02f;
    [SerializeField] private float breathingFrequency = 1.5f;
    [SerializeField] private float breathingHorizontalRatio = .5f;
    [SerializeField] private float breathingTransitionSpeed = 2f;
    [SerializeField] private float breathingWeight = .2f;
    private float currentBreathingWeight;

    [Header("Move Sway")]
    [SerializeField] private float moveAmplitudeX = .05f;
    [SerializeField] private float moveAmplitudeY = .03f;
    [SerializeField] private float walkFrequency = 7f;
    [SerializeField] private float sprintFrequency = 10f;
    [SerializeField] private float moveTransitionSpeed = 5f;
    [SerializeField] private float verticalFrequncyRatio = 2f;
    [SerializeField] private float moveWeight = 1f;
    private float currentMoveWeight;
    private float moveFrequency;

    private GameInput gameInput;
    private Vector3 originalPos;

    public void Initialized(GameInput gameInput, Vector3 originalPos)
    {
        this.gameInput = gameInput;
        this.originalPos = originalPos;
    }

    public void HandleSway(Transform weapon, PlayerController playerController,  bool isShooting, bool isAiming)
    {
        if (isShooting) return;

        float targetSwayMultiplier = isAiming ? swayMultiplier : 1f;
        Vector2 mouseInput = gameInput.GetMouseInput();
        Vector2 moveInput = gameInput.GetMoveInputNormalized();

        Vector3 mouseSway = GetMouseSway(mouseInput);
        Vector3 idleSway = GetIdleSway(mouseInput, moveInput) * targetSwayMultiplier;
        Vector3 moveSway = GetMoveSway(moveInput, playerController.IsSprinting) * targetSwayMultiplier;

        weapon.localPosition = Vector3.Lerp(weapon.localPosition, originalPos + mouseSway + idleSway + moveSway, smoothTime * Time.deltaTime);
    }

    private Vector3 GetMouseSway(Vector2 mouseInput)
    {
        float mouseX = Mathf.Clamp(mouseInput.x, -swayClamp, swayClamp);
        float mouseY = Mathf.Clamp(mouseInput.y, -swayClamp, swayClamp);
       
        return new(-mouseX, -mouseY, 0f);
    }

    private Vector3 GetIdleSway(Vector2 mouseInput, Vector2 moveInput)
    {
        bool isIdle = moveInput == Vector2.zero && mouseInput == Vector2.zero;
        currentBreathingWeight = Mathf.Lerp(currentBreathingWeight, isIdle ? breathingWeight : 0, breathingTransitionSpeed * Time.deltaTime);
        float idleSwayX = Mathf.Cos(Time.time * breathingFrequency * breathingHorizontalRatio) * breathingAmplitude * breathingHorizontalRatio * currentBreathingWeight;
        float idleSwayY = Mathf.Sin(Time.time * breathingFrequency) * breathingAmplitude * currentBreathingWeight;

        return new(idleSwayX, idleSwayY, 0f);
    }

    private Vector3 GetMoveSway(Vector2 moveInput, bool isSprinting)
    {
        bool isMove = moveInput != Vector2.zero;
        currentMoveWeight = Mathf.Lerp(currentMoveWeight, isMove ? moveWeight: 0, moveTransitionSpeed * Time.deltaTime);
        moveFrequency = isSprinting ? sprintFrequency : walkFrequency;

        float moveSwayX = Mathf.Cos(Time.time * moveFrequency) * moveAmplitudeX * currentMoveWeight;
        float moveSwayY = Mathf.Sin(Time.time * moveFrequency * verticalFrequncyRatio) * moveAmplitudeY * currentMoveWeight;

        return new(moveSwayX, moveSwayY, 0f);
    }
}

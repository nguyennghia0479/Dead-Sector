using UnityEngine;

[System.Serializable]
public class FullBodyAnimations
{
    [SerializeField] private Animator animator;
    [SerializeField] private float dampTime = .01f;
    [SerializeField] private float animationSpeedMultiplier = 1.5f;

    private static readonly int HorizontalHash = Animator.StringToHash("horizontal");
    private static readonly int VerticalHash = Animator.StringToHash("vertical");
    private static readonly int IsCrouchingHash = Animator.StringToHash("isCrouching");
    private static readonly int IsTurningHash = Animator.StringToHash("isTurning");
    private static readonly int MouseXHash = Animator.StringToHash("mouseX");

    public void HandleAnimations(Vector2 moveInput, Vector2 mouseInput, bool isCrouching, bool isSprinting)
    {
        if (animator == null) return;

        animator.SetFloat(HorizontalHash, moveInput.x, dampTime, Time.deltaTime);
        animator.SetFloat(VerticalHash, moveInput.y, dampTime, Time.deltaTime);
        animator.SetBool(IsCrouchingHash, isCrouching);
        animator.speed = isSprinting ? animationSpeedMultiplier : 1f;

        float mouseX = Mathf.Clamp(mouseInput.x, -1f, 1f);
        animator.SetBool(IsTurningHash, Mathf.Abs(mouseX) > .1f);
        animator.SetFloat(MouseXHash, mouseX);
    }
}

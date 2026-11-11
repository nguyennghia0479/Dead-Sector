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
    private static readonly int DeathHash_1 = Animator.StringToHash("death_1");
    private static readonly int DeathHash_2 = Animator.StringToHash("death_2");
    private static readonly int CrouchDeathHash = Animator.StringToHash("crouchDeath");

    public void HandleLocomotions(Vector2 moveInput, Vector2 mouseInput, bool isCrouching, bool isSprinting)
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

    public void HandleDeathAnimation(bool isCrouching)
    {
        if (animator == null) return;
        animator.applyRootMotion = true;
        if (isCrouching)
            animator.SetTrigger(CrouchDeathHash);
        else
        {
            if (Random.Range(0, 2) == 0)
                animator.SetTrigger(DeathHash_1);
            else
                animator.SetTrigger(DeathHash_2);
        }
    }
}

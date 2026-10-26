using UnityEngine;

[System.Serializable]
public class FullBodyAnimations
{
    [SerializeField] private Animator animator;
    [SerializeField] private float dampTime = .01f;

    private static readonly int HorizontalHash = Animator.StringToHash("horizontal");
    private static readonly int VerticalHash = Animator.StringToHash("vertical");
    private static readonly int IsCrouchingHash = Animator.StringToHash("isCrouching");

    public void HandleAnimations(Vector2 moveInput, bool isCrouching)
    {
        if (animator == null) return;

        animator.SetFloat(HorizontalHash, moveInput.x, dampTime, Time.deltaTime);
        animator.SetFloat(VerticalHash, moveInput.y, dampTime, Time.deltaTime);
        animator.SetBool(IsCrouchingHash, isCrouching);
    }
}

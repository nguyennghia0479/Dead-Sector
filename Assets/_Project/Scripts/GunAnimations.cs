using UnityEngine;

[System.Serializable]
public class GunAnimations
{
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private AnimatorOverrideController gunAnimatorOverride;

    private static readonly int ReloadHash = Animator.StringToHash("reload");

    public void Initialized()
    {
        if (gunAnimator != null && gunAnimatorOverride != null)
            gunAnimator.runtimeAnimatorController = gunAnimatorOverride;
    }

    public void PlayReloadAnimation()
    {
        gunAnimator.SetTrigger(ReloadHash);
    }
    
}

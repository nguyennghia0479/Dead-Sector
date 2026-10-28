using UnityEngine;

[System.Serializable]
public class GunAnimations
{
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private AnimatorOverrideController gunAnimatorOverride;

    private static readonly int ReloadHash = Animator.StringToHash("reload");
    private static readonly int IsLoopReloadHash = Animator.StringToHash("isLoopReload");
    private static readonly int ReloadLoopHash = Animator.StringToHash("reloadLoop");
    private static readonly int ReloadFinishedHash = Animator.StringToHash("reloadFinished");
    private static readonly int UnequipHash = Animator.StringToHash("unequip");

    public void Initialized()
    {
        if (gunAnimator != null && gunAnimatorOverride != null)
            gunAnimator.runtimeAnimatorController = gunAnimatorOverride;
    }

    public void PlayReloadAnimation(bool isIncremental)
    {
        gunAnimator.SetBool(IsLoopReloadHash, isIncremental);
        gunAnimator.SetTrigger(ReloadHash);
    }

    public void PlayReloadLoopAnimation()
    {
        gunAnimator.SetTrigger(ReloadLoopHash);
    }

    public void PlayReloadFinishedAnimation()
    {
        gunAnimator.SetTrigger(ReloadFinishedHash);
    }
    
    public void PlayUnequipAnimation()
    {
        gunAnimator.SetTrigger(UnequipHash);
    }
}

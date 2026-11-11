using UnityEngine;

[System.Serializable]
public class GunAnimations
{
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private AnimatorOverrideController gunAnimatorOverride;

    private static readonly int ReloadHash = Animator.StringToHash("reload");
    private static readonly int IsIncrementalHash = Animator.StringToHash("isIncremental");
    private static readonly int ReloadLoopHash = Animator.StringToHash("reloadLoop");
    private static readonly int ReloadFinishedHash = Animator.StringToHash("reloadFinished");
    private static readonly int HideHash = Animator.StringToHash("hide");
    private static readonly int IsSwitchGunHash = Animator.StringToHash("isSwitchGun");

    public void Initialized()
    {
        if (gunAnimator != null && gunAnimatorOverride != null)
            gunAnimator.runtimeAnimatorController = gunAnimatorOverride;
    }

    public void PlayReloadAnimation(bool isIncremental)
    {
        gunAnimator.ResetTrigger(ReloadHash);
        gunAnimator.ResetTrigger(ReloadLoopHash);
        gunAnimator.ResetTrigger(ReloadFinishedHash);

        gunAnimator.SetBool(IsIncrementalHash, isIncremental);
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
    
    public void PlayUnequipAnimation(bool isSwitchGun)
    {
        gunAnimator.SetBool(IsSwitchGunHash, isSwitchGun);
        gunAnimator.SetTrigger(HideHash);
    }

    public bool IsSwitchGun() => gunAnimator.GetBool(IsSwitchGunHash);
}

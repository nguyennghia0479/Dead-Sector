using UnityEngine;

public class GunAnimationBridge : MonoBehaviour
{
    private GunController gunController;

    private void Awake()
    {
        gunController = GetComponent<GunController>();
    }

    public void CallDrawGun()
    {
        gunController.SetGunActivated(true);
    }

    public void CallHideGun()
    {
        bool isSwitchGun = gunController.GunAnimations.IsSwitchGun();
        if (isSwitchGun)
            gunController.SetGunActivated(false);
        else
        {
            gunController.Player.StartThrowable();
            gunController.gameObject.SetActive(false);
        }
    }

    public void CallOnShellInserted()
    {
        gunController.ReloadLoop();
    }

    public void CallOnShellInsertedSound()
    {
        gunController.PlayReloadLoopSound();
    }

    public void CallReloadFinished()
    {
        gunController.ReloadFinished();
    }

}

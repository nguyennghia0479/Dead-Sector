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
        gunController.SetGunActivated(false);
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

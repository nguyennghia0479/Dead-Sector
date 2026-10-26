using UnityEngine;

public class GunAnimationBridge : MonoBehaviour
{
    private GunController gunController;

    private void Awake()
    {
        gunController = GetComponent<GunController>();
    }

    public void CallGunEquiped()
    {
        gunController.GunActivated(true);
    }

    public void CallGunUnequiped()
    {
        gunController.GunActivated(false);
    }

    public void CallReloadFinished()
    {
        gunController.ReloadFinished();
    }
}

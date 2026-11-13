using UnityEngine;

[System.Serializable]
public class GunAudio
{
    [SerializeField] private AudioSource gunSource;
    private AudioClip drawClip;
    private AudioClip hideClip;
    private AudioClip noAmmoClip;
    private AudioClip shootClip;
    private AudioClip reloadClip;
    private AudioClip reloadLoopClip;
    private AudioClip reloadnEndClip;

    public void Initialized(GunSO gunSO)
    {
        drawClip = gunSO.DrawClip;
        hideClip = gunSO.HideClip;
        noAmmoClip = gunSO.NoAmmoClip;
        shootClip = gunSO.ShootClip;
        reloadClip = gunSO.ReloadClip;
        reloadLoopClip = gunSO.ReloadLoopClip;
        reloadnEndClip = gunSO.ReloadEndClip;
    }

    public void PlayDrawSFX()
    {
        if (gunSource && drawClip)
            gunSource.PlayOneShot(drawClip);
    }

    public void PlayHideSFX()
    {
        if (gunSource && hideClip)
            gunSource.PlayOneShot(hideClip);
    }

    public void PlayGunShootSFX()
    {
        if (gunSource && shootClip)
            gunSource.PlayOneShot(shootClip);
    }

    public void PlayNoAmmoSFX()
    {
        if (gunSource && noAmmoClip)
            gunSource.PlayOneShot(noAmmoClip);
    }

    public void PlayGunReloadSFX()
    {
        if (gunSource && reloadClip)
            gunSource.PlayOneShot(reloadClip);
    }

    public void PlayGunReloadLoopSFX()
    {
        if (gunSource && reloadLoopClip)
            gunSource.PlayOneShot(reloadLoopClip);
    }

    public void PlayGunReloadEndSFX()
    {
        if (gunSource && reloadnEndClip)
            gunSource.PlayOneShot(reloadnEndClip);
    }
}

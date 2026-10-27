using System.Collections;
using UnityEngine;

public class MuzzleFlashEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private Light light;
    
    private WaitForSeconds waitTime;

    private void Awake()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            var main = ps.main;
            main.playOnAwake = false;
        }

        light.enabled = false;
        waitTime = new WaitForSeconds(particleSystems[0].main.startLifetime.constant);
    }

    public void Play()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }

        StopAllCoroutines();
        StartCoroutine(FlashLightRoutine());
    }

    private IEnumerator FlashLightRoutine()
    {
        light.enabled = true;
        yield return waitTime;
        light.enabled = false;
    }
}

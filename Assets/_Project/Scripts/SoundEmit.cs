using System.Collections;
using UnityEngine;

public class SoundEmit : MonoBehaviour
{
    private PooledObject pooledObject;
    private AudioSource audioSource;

    private void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip, Vector3 position, float volume)
    {
        if (audioSource == null) return;
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.transform.position = position;
        audioSource.Play();

        StartCoroutine(SelfDestroyRoutine());
    }

    private IEnumerator SelfDestroyRoutine()
    {
        while (audioSource.isPlaying)
            yield return null;

        if (pooledObject != null)
            pooledObject.ReleasePool(gameObject);
        else
            Destroy(gameObject);
    }
}

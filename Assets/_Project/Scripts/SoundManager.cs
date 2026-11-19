using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject soundEmmitPrefab;

    private void OnEnable()
    {
        GameEvents.OnBulletHit += HandleBulletHit;
    }

    private void OnDisable()
    {
        GameEvents.OnBulletHit -= HandleBulletHit;
    }

    private void PlaySound(AudioClip clip, Vector3 position, float volume)
    {
        GameObject soundEmitPool = ObjectPoolManager.Instance.GetPool(soundEmmitPrefab, position, Quaternion.identity);
        if (soundEmitPool != null && soundEmitPool.TryGetComponent<SoundEmit>(out var newSoundEmit))
            newSoundEmit.PlaySFX(clip, position, volume);
    }

    private void HandleBulletHit(AudioClip clip, Vector3 position, float volume)
    {
        PlaySound(clip, position, volume);
    }
}

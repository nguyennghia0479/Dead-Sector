using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundDatabaseSO soundDatabaseSO;
    [SerializeField] private GameObject soundEmmitPrefab;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        UIEvents.OnButtonClicked += HandleButtonClicked;

        GameEvents.OnItemPicked += HandleItemPicked;
        GameEvents.OnWaveStarted += HandleWaveStarted;
        GameEvents.OnBulletHit += HandleBulletHit;
    }

    private void OnDisable()
    {
        UIEvents.OnButtonClicked -= HandleButtonClicked;

        GameEvents.OnItemPicked -= HandleItemPicked;
        GameEvents.OnWaveStarted -= HandleWaveStarted;
        GameEvents.OnBulletHit -= HandleBulletHit;
    }

    private void PlaySound(AudioClip clip, Vector3 position, float volume)
    {
        if (position == Vector3.zero)
        {
            audioSource.volume = volume;
            audioSource.PlayOneShot(clip);
            return;
        }

        GameObject soundEmitPool = ObjectPoolManager.Instance.GetPool(soundEmmitPrefab, position, Quaternion.identity);
        if (soundEmitPool != null && soundEmitPool.TryGetComponent<SoundEmit>(out var newSoundEmit))
            newSoundEmit.PlaySFX(clip, position, volume);
    }

    private void HandleButtonClicked(float volume)
    {
        AudioClip clip = soundDatabaseSO.GetRandomClipByType(SoundType.ButtonClicked);
        if (clip != null)
            PlaySound(clip, Vector3.zero, volume);
    }

    private void HandleItemPicked(float volume)
    {
        AudioClip clip = soundDatabaseSO.GetRandomClipByType(SoundType.PickupItem);
        if (clip != null) 
            PlaySound(clip, Vector3.zero, volume);
    }

    private void HandleWaveStarted(float volume)
    {
        AudioClip clip = soundDatabaseSO.GetRandomClipByType(SoundType.ZombieScream);
        if (clip != null)
            PlaySound(clip, Vector3.zero, volume);
    }

    private void HandleBulletHit(SoundType soundType, Vector3 position, float volume)
    {
        AudioClip clip = soundDatabaseSO.GetRandomClipByType(soundType);
        if (clip != null)
            PlaySound(clip, position, volume);
    }
}

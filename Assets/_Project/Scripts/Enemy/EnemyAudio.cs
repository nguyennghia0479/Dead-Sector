using UnityEngine;

[System.Serializable]
public class EnemyAudio
{
    [Header("Enemy SFX")]
    [SerializeField] private float delay = 1.5f;
    [SerializeField] private AudioSource enemySource;
    [SerializeField] private AudioClip[] growlClips;
    [SerializeField] private AudioClip[] chaseClips;
    [SerializeField] private AudioClip[] hurtClips;
    [SerializeField] private AudioClip[] deathClips;
    private AudioClip currentClipPlay;

    public void PlayGrowlSFX()
    {
        PlaySFX(growlClips);
    }

    public void PlayChaseSFX()
    {
        PlaySFX(chaseClips);
    }

    public void PlayHurtSFX()
    {
        enemySource.Stop();
        PlaySFX(hurtClips);
    }

    public void PlayDeathSFX()
    {
        enemySource.Stop();
        PlaySFX(deathClips);
    }

    private void PlaySFX(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip clip;
        if (clips.Length > 1)
            clip = clips[Random.Range(0, clips.Length)];
        else
            clip = clips[0];

        currentClipPlay = clip;
        enemySource.PlayOneShot(clip);
    }

    public float GetDelayTimeWithClipLength(bool useClipLength)
    {
        if (currentClipPlay != null && useClipLength)
            return delay + currentClipPlay.length;
        return delay;
    }

    public void StopSFX() => enemySource.Stop();

    public bool IsPlaying() => enemySource.isPlaying;
}

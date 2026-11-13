using UnityEngine;

[System.Serializable]
public class PlayerAudio
{
    [Header("Footsteps SFX")]
    [SerializeField] private AudioSource footSource;
    [SerializeField] private AudioClip[] footClips;
    [SerializeField] private AudioClip landingClip;
    [Space]
    [SerializeField] private float walkInterval = .4f;
    [SerializeField] private float sprintInterval = .3f;
    [SerializeField] private float crouchInterval = .6f;
    private float intervalTimer;
    private int currentIndex;
    private bool wasGrounded = true;

    [Header("Others SFX")]
    [SerializeField] private AudioSource playerSource;
    [SerializeField] private AudioClip[] healClips;
    [SerializeField] private AudioClip[] hurtClips;
    [SerializeField] private AudioClip[] deadClips;
    [SerializeField] private AudioClip[] breathClips;

    public void HandleFootsteps(bool isMoving, bool isSprinting, bool isCrouching, bool isGrounded)
    {
        if (footSource == null || footClips == null || footClips.Length == 0)
            return;

        if (!isMoving || !isGrounded)
        {
            if (intervalTimer > 0)
                PlayFootstepSFX();
            
            intervalTimer = 0f;
            return;
        }

        float currentInterval = isSprinting ? sprintInterval : isCrouching ? crouchInterval : walkInterval;
        intervalTimer += Time.deltaTime;
        if (intervalTimer > currentInterval)
        {
            PlayFootstepSFX();
            intervalTimer = 0f;
        }
    }

    public void HandleLanding(bool isGrounded)
    {
        if (footSource == null || landingClip == null) return;

        if (!wasGrounded && isGrounded)
            footSource.PlayOneShot(landingClip);

        wasGrounded = isGrounded;
    }

    private void PlayFootstepSFX()
    {
        if (footClips == null || footClips.Length == 0) return;

        AudioClip footClip = footClips[currentIndex];
        currentIndex = (currentIndex + 1) % footClips.Length;
        footSource.PlayOneShot(footClip);
    }

    public void PlayHealSFX()
    {
        PlayRandomClip(healClips);
    }

    public void PlayHurtSFX()
    {
        PlayRandomClip(hurtClips);
    }

    public void PlayDeadSFX()
    {
        PlayRandomClip(deadClips);
    }

    public void PlayBreathSFX()
    {
        PlayRandomClip(breathClips);
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip clip;
        if (clips.Length > 1)
            clip = clips[Random.Range(0, clips.Length)];
        else
            clip = clips[0];
        playerSource.PlayOneShot(clip);
    }
}

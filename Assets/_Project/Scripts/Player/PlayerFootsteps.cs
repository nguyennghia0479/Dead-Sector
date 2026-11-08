using UnityEngine;

[System.Serializable]
public class PlayerFootsteps
{
    [Header("Footsteps Settings")]
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
        AudioClip footClip = footClips[currentIndex];
        currentIndex = (currentIndex + 1) % footClips.Length;
        footSource.PlayOneShot(footClip);
    }
}

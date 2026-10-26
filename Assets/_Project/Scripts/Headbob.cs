using UnityEngine;

[System.Serializable]
public class HeadBob
{
    [Header("Heab Bob Settings")]
    [SerializeField] private float walkBobAmount = .05f;
    [SerializeField] private float walkBobSpeed = 8f;
    [SerializeField] private float sprintBobAmount = .08f;
    [SerializeField] private float sprintBobSpeed = 12f;
    [SerializeField] private float horizontalBobMultiplier = .5f;
    [SerializeField] private float verticalBobMultiplier = 2f;
    [SerializeField] private float smoothBobSpeed = 8f;

    private Vector3 originalPlayerCamPos;
    private float bobTimer;

    public void Initialized(Vector3 originalPlayerCamPos)
    {
        this.originalPlayerCamPos = originalPlayerCamPos;
    }

    public void HandleHeadBob(Transform playerCamera, bool isMoving, bool isGrounded, bool isSprinting)
    {
        if (isMoving && isGrounded)
        {
            float bobAmount = isSprinting ? sprintBobAmount : walkBobAmount;
            float bobSpeed = isSprinting ? sprintBobSpeed : walkBobSpeed;

            bobTimer += bobSpeed * Time.deltaTime;
            float bobX = Mathf.Cos(bobTimer) * bobAmount * horizontalBobMultiplier;
            float bobY = Mathf.Sin(bobTimer * verticalBobMultiplier) * bobAmount;

            Vector3 headBobMovement = new(bobX, bobY, 0f);
            Vector3 targetPosition = originalPlayerCamPos + headBobMovement;
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, targetPosition, smoothBobSpeed * Time.deltaTime);
        }
        else
        {
            bobTimer = 0f;
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, originalPlayerCamPos, smoothBobSpeed * Time.deltaTime);
        }
    }
}

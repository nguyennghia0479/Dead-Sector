using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float smoothSpeed = 10f;
    private float currentSpeed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedForce = -2f;
    [SerializeField] private bool isGrounded;
    private Vector3 playerVelocity;

    [Header("Crouch Settings")]
    [SerializeField] private float originalHeight;
    [SerializeField] private float crouchHeight;
    [SerializeField] private bool isCrouching;

    [Header("Moving Status")]
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isMoving;

    [Header("References")]
    [SerializeField] private Transform gunHolder;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerCamera playerCam;
    [SerializeField] private HeadBob headBob;
    [SerializeField] private FullBodyAnimations bodyAnimations;

    private CharacterController controller;
    private GunController gunAssigned;
    private WeaponManager weaponManager;
    private GameInput gameInput;
    private Vector2 moveInput;
    private Vector3 cameraRecoilRotation;
    private bool isAiming;

    public bool IsCrouching => isCrouching;
    public bool IsSprinting => isSprinting;
    public bool IsMoving => isMoving;
    public GameInput GameInput => gameInput;
    public WeaponManager WeaponManager => weaponManager;
    public Transform GunHolder => gunHolder;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        gameInput = GetComponent<GameInput>();
        weaponManager = GetComponent<WeaponManager>();

        currentSpeed = walkSpeed;
        originalHeight = controller.height;
        crouchHeight = originalHeight / 2f;
    }

    private void Start()
    {
        playerCam.Initialized();
        headBob.Initialized(mainCamera.transform.localPosition);
    }

    private void OnEnable()
    {
        gameInput.OnJumpPressed += HandleJumpPressed;
        gameInput.OnCrouchPressed += HandleCrouchPressed;
        gameInput.OnSprintPressed += HandleSprintPressed;
        gameInput.OnSprintReleased += HandleSprintRelease;
        gameInput.OnShootPressed += HandleShootPressed;
        gameInput.OnShootReleased += HandleShootReleased;
        gameInput.OnReloadPressed += HandleReloadPressed;
        gameInput.OnAimPressed += HandleAimPressed;
    }

    private void OnDisable()
    {
        gameInput.OnJumpPressed -= HandleJumpPressed;
        gameInput.OnCrouchPressed -= HandleCrouchPressed;
        gameInput.OnSprintPressed -= HandleSprintPressed;
        gameInput.OnSprintReleased -= HandleSprintRelease;
        gameInput.OnShootPressed -= HandleShootPressed;
        gameInput.OnShootReleased -= HandleShootReleased;
        gameInput.OnReloadPressed -= HandleReloadPressed;
        gameInput.OnAimPressed -= HandleAimPressed;
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        HandleVerticalVelocity();
        HandleFinalMove();
        HandleRotation();
        HandleCrouch();
        HandleHeadBob();
        HandleFullBodyAnimation();
    }

    private void HandleVerticalVelocity()
    {
        if (isGrounded && playerVelocity.y <= 0)
            playerVelocity.y = groundedForce;

        playerVelocity.y += gravity * Time.deltaTime;
    }

    private void HandleFinalMove()
    {
        float targetSpeed = isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, smoothSpeed * Time.deltaTime);

        moveInput = gameInput.GetMoveInputNormalized();
        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 finalMove = currentSpeed * movement + playerVelocity.y * Vector3.up;

        isMoving = controller.velocity.magnitude > .1f;
        controller.Move(finalMove * Time.deltaTime);
    }

    private void HandleRotation()
    {
        playerCam.HandleRotation(transform, mainCamera.transform, gameInput, cameraRecoilRotation);
    }

    public void UpdateCameraRecoilRotation(Vector3 cameraRecoilRotation) => this.cameraRecoilRotation = cameraRecoilRotation;

    private void HandleCrouch()
    {
        float targetHeight = isCrouching ? crouchHeight : originalHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, smoothSpeed * Time.deltaTime);
    }

    private void HandleHeadBob()
    {
        headBob.HandleHeadBob(mainCamera.transform, IsMoving, isGrounded, isSprinting);
    }

    private void HandleFullBodyAnimation()
    {
        bodyAnimations.HandleAnimations(moveInput, gameInput.GetMouseInput(), isCrouching, isSprinting);
    }

    public void AssginGun(GunController gun)
    {
        gunAssigned = gun;
        if (isAiming)
        {
            isAiming = !isAiming;
            gunAssigned.SetGunAiming(isAiming);
        }
    }

    private void HandleJumpPressed()
    {
        if (isGrounded)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * groundedForce * gravity);
    }

    private void HandleCrouchPressed()
    {
        if (isGrounded)
            isCrouching = !isCrouching;
    }

    private void HandleSprintPressed()
    {
        if (isGrounded)
        {
            isCrouching = false;
            isSprinting = true;
        }
    }

    private void HandleSprintRelease()
    {
        isSprinting = false;
    }

    private void HandleShootPressed()
    {
        if (gunAssigned.GunActivated)
            gunAssigned.SetGunShooting(true);
    }

    private void HandleShootReleased()
    {
        if (gunAssigned.GunActivated)
            gunAssigned.SetGunShooting(false);
    }

    private void HandleReloadPressed()
    {
        if (gunAssigned.GunActivated)
            gunAssigned.HandleReload();
    }

    private void HandleAimPressed()
    {
        if (gunAssigned.GunActivated)
        {
            isAiming = !isAiming;
            gunAssigned.SetGunAiming(isAiming);
        }
    }
}

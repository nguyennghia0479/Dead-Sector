using UnityEngine;

public class PlayerController : EntityController
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

    [Header("Sprint Settings")]
    [SerializeField] private float maxSprintTime = 5f;
    [SerializeField] private float sprintRecoveryRate = 1f;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isMoving;
    [SerializeField] private float sprintTimer;

    [Header("Throw Settings")]
    [SerializeField] private float throwForce = 12f;
    [SerializeField] private Transform throwSpawn;

    [Header("References")]
    [SerializeField] private Transform gunHolder;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerCamera playerCam;
    [SerializeField] private HeadBob headBob;
    [SerializeField] private FullBodyAnimations bodyAnimations;
    [SerializeField] private PlayerAudio playerAudio;

    private PlayerHealth playerHealth;
    private CharacterController controller;
    private WeaponManager weaponManager;
    private GunController gunAssigned;
    private ThrowableController throwableAssigned;
    private GameInput gameInput;
    private ScopeCamera scopeCamera;
    private ADS aimDotSight;
    private Vector2 moveInput;
    private Vector3 cameraRecoilRotation;
    private bool isAiming;
    private float defaultMainCamFOV;

    public bool IsCrouching => isCrouching;
    public bool IsSprinting => isSprinting;
    public bool IsMoving => isMoving;
    public PlayerHealth PlayerHealth => playerHealth;
    public GameInput GameInput => gameInput;
    public WeaponManager WeaponManager => weaponManager;
    public GunController GunActivated => gunAssigned;
    public ThrowableController ThrowableActivated => throwableAssigned;
    public ScopeCamera ScopeCamera => scopeCamera;
    public ADS ADS => aimDotSight;
    public Transform GunHolder => gunHolder;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        controller = GetComponent<CharacterController>();
        gameInput = GetComponent<GameInput>();
        weaponManager = GetComponent<WeaponManager>();
        throwableAssigned = GetComponentInChildren<ThrowableController>();
        scopeCamera = GetComponentInChildren<ScopeCamera>();
        aimDotSight = GetComponentInChildren<ADS>();

        currentSpeed = walkSpeed;
        originalHeight = controller.height;
        crouchHeight = originalHeight / 2f;
        defaultMainCamFOV = mainCamera.fieldOfView;
    }

    private void Start()
    {
        playerCam.Initialized();
        headBob.Initialized(mainCamera.transform.localPosition);
        throwableAssigned.Initialized(throwForce, throwSpawn);
        throwableAssigned.gameObject.SetActive(false);
        UIManager.Instance.HUDInitialzied(this);
        Initialized(playerHealth);
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
        GameEvents.OnPlayerHealed += HandlePlayerHealed;
        GameEvents.OnPlayerHit += HandlePlayerHit;

        gameInput.OnJumpPressed += HandleJumpPressed;
        gameInput.OnCrouchPressed += HandleCrouchPressed;
        gameInput.OnSprintPressed += HandleSprintPressed;
        gameInput.OnSprintReleased += HandleSprintRelease;
        gameInput.OnShootPressed += HandleShootPressed;
        gameInput.OnShootReleased += HandleShootReleased;
        gameInput.OnReloadPressed += HandleReloadPressed;
        gameInput.OnAimPressed += HandleAimPressed;
        gameInput.OnThrown += HandleThrowPressed;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
        GameEvents.OnPlayerHealed -= HandlePlayerHealed;
        GameEvents.OnPlayerHit -= HandlePlayerHit;

        gameInput.OnJumpPressed -= HandleJumpPressed;
        gameInput.OnCrouchPressed -= HandleCrouchPressed;
        gameInput.OnSprintPressed -= HandleSprintPressed;
        gameInput.OnSprintReleased -= HandleSprintRelease;
        gameInput.OnShootPressed -= HandleShootPressed;
        gameInput.OnShootReleased -= HandleShootReleased;
        gameInput.OnReloadPressed -= HandleReloadPressed;
        gameInput.OnAimPressed -= HandleAimPressed;
        gameInput.OnThrown -= HandleThrowPressed;
    }

    private void Update()
    {
        if (playerHealth.IsDeath) return;

        isGrounded = controller.isGrounded;
        HandleVerticalVelocity();
        HandleSprint();
        HandleFinalMove();
        HandleRotation();
        HandleCrouch();
        HandleHeadBob();
        HandleLocomotions();
        HandleMainCameraFOV();
        HandleFootstepsSFX();
        HandleLandingSFX();
        HandleBurnHit();
    }

    private void HandleVerticalVelocity()
    {
        if (isGrounded && playerVelocity.y <= 0)
            playerVelocity.y = groundedForce;

        playerVelocity.y += gravity * Time.deltaTime;
    }

    private void HandleSprint()
    {
        sprintTimer = isSprinting ? sprintTimer + Time.deltaTime : sprintTimer - (Time.deltaTime * sprintRecoveryRate);
        if (isSprinting && sprintTimer > maxSprintTime)
        {
            isSprinting = false;
            playerAudio.PlayBreathSFX();
        }
    }

    private void HandleFinalMove()
    {
        float targetSpeed = isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, smoothSpeed * Time.deltaTime);

        moveInput = gameInput.GetMoveInputNormalized();
        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 finalMove = currentSpeed * movement + playerVelocity.y * Vector3.up;

        Vector3 velocity = controller.velocity;
        velocity.y = 0f;
        isMoving = velocity.magnitude > .1f;
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

    private void HandleLocomotions()
    {
        bodyAnimations.HandleLocomotions(moveInput, gameInput.GetMouseInput(), isCrouching, isSprinting);
    }

    private void HandleFootstepsSFX()
    {
        playerAudio.HandleFootsteps(isMoving, isSprinting, isCrouching, isGrounded);
    }

    private void HandleLandingSFX()
    {
        playerAudio.HandleLanding(isGrounded);
    }

    private void HandleMainCameraFOV()
    {
        if (gunAssigned != null)
        {
            float targetFOV = isAiming ? gunAssigned.GunAiming.AimMainCamFOV : defaultMainCamFOV;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, smoothSpeed * Time.deltaTime);
        }
    }

    public void CancelAimingIfNeeded()
    {
        if (isAiming)
        {
            isAiming = !isAiming;
            gunAssigned.SetGunAiming(isAiming);
        }
    }

    public void AssginGun(GunController gun)
    {
        gunAssigned = gun;
        CancelAimingIfNeeded();
        playerCam.HandleSniperAiming(isAiming && gunAssigned.IsSniper);
    }

    public void StartThrowable()
    {
        throwableAssigned.gameObject.SetActive(true);
    }

    private void HandlePlayerDeath()
    {
        playerAudio.PlayDeadSFX();
        gameInput.DisableInput();
        gunAssigned.gameObject.SetActive(false);
        bodyAnimations.HandleDeathAnimation(IsCrouching);
        mainCamera.enabled = false;
    }

    private void HandlePlayerHealed()
    {
        if (!playerHealth.IsDeath)
            playerAudio.PlayHealSFX();
    }

    private void HandlePlayerHit()
    {
        if (!playerHealth.IsDeath)
            playerAudio.PlayHurtSFX();
    }

    private void HandleJumpPressed()
    {
        if (isCrouching)
        {
            isCrouching = false;
            return;
        }

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

            if (sprintTimer < 0)
                sprintTimer = 0;
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
        {
            CancelAimingIfNeeded();
            gunAssigned.HandleReload();
        }
    }

    private void HandleAimPressed()
    {
        if (gunAssigned.GunActivated && !gunAssigned.IsReloading)
        {
            isAiming = !isAiming;
            gunAssigned.SetGunAiming(isAiming);
            playerCam.HandleSniperAiming(isAiming && gunAssigned.IsSniper);
        }
    }

    private void HandleThrowPressed(ThrowableType throwableType)
    {
        if (throwableAssigned.TryToThrow(throwableType))
            gunAssigned.PlayUnequipAnimation(false);
    }
}

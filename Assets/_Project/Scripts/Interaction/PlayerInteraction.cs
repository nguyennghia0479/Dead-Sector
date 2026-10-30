using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public event Action<IInteractable> OnInteractableChanged;

    [Header("Detection Settings")]
    [SerializeField] private float detectInterval = .1f;
    [SerializeField] private float detectionRange;
    [SerializeField] private LayerMask interactionLayer;

    private PlayerController player;
    private GameInput gameInput;
    private Camera mainCam;
    private IInteractable currentInteractable;
    private float holdTimer;
    private bool interactionCompleted;
    private bool isInteractionPressed;
    private float nextDetectTimer;

    public GameInput GameInput => gameInput;
    public IInteractable CurrentInteractable => currentInteractable;

    private void Awake()
    {
        mainCam = Camera.main;
        player = GetComponent<PlayerController>();
        gameInput = player.GameInput;
    }

    private void Start()
    {
        UIManager.Instance.InteractionInitialized(this);
    }

    private void OnEnable()
    {
        gameInput.OnInteractPressed += HandleInteractPressed;
        gameInput.OnInteractReleased += HandleInteractRelease;
    }

    private void OnDisable()
    {
        gameInput.OnInteractPressed -= HandleInteractPressed;
        gameInput.OnInteractReleased -= HandleInteractRelease;
    }

    private void Update()
    {
        nextDetectTimer += Time.deltaTime;
        if (nextDetectTimer >= detectInterval)
        {
            nextDetectTimer = 0;
            HandleDetectInteractable();
        }

        HandleInteraction();
    }

    public void SetCurrentInteractable(IInteractable interactable)
    {
        if (currentInteractable == interactable) return;

        ResetHold();

        currentInteractable?.SetHighlight(false);
        currentInteractable = interactable;
        OnInteractableChanged?.Invoke(currentInteractable);
    }

    public float GetHoldProgress()
    {
        if (currentInteractable != null && !currentInteractable.IsHoldInteract)
            return 0f;

        return Mathf.Clamp01(holdTimer / currentInteractable.HoldDuration);
    }

    private void HandleDetectInteractable()
    {
        IInteractable detectedInteractable = null;
        Ray ray = mainCam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange, interactionLayer))
        {
            if (hit.collider == null)
                return;

            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null && interactable.CanInteract)
            {
                detectedInteractable = interactable;
                detectedInteractable.SetHighlight(true);
            }
        }

        SetCurrentInteractable(detectedInteractable);
    }

    private void HandleInteraction()
    {
        if (currentInteractable == null || !currentInteractable.CanInteract)
        {
            ResetHold();
            return;
        }

        if (!currentInteractable.IsHoldInteract)
        {
            if (isInteractionPressed)
                Interact();
            return;
        }

        if (isInteractionPressed)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= currentInteractable.HoldDuration && !interactionCompleted)
            {
                interactionCompleted = true;
                Interact();
            }
        }

        if (!isInteractionPressed)
            ResetHold();
    }

    private void Interact()
    {
        IInteractable interactable = currentInteractable;
        SetCurrentInteractable(null);
        interactable.OnInteract(player);
    }

    private void ResetHold()
    {
        holdTimer = 0f;
        interactionCompleted = false;
    }

    private void HandleInteractPressed()
    {
        isInteractionPressed = true;
    }

    private void HandleInteractRelease()
    {
        isInteractionPressed = false;
    }
}

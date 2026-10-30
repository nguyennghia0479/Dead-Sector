using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private Image holdProgressFill;
    [SerializeField] private TMP_Text interactionText;

    private string keyText;
    private IInteractable currentInteractable;
    private PlayerInteraction playerInteraction;


    private void Awake()
    {
        interactionPanel.SetActive(false);
        holdProgressFill.fillAmount = 0f;
    }

    private void OnEnable()
    {
        if (playerInteraction != null)
            playerInteraction.OnInteractableChanged += HandleInteractableChanged;
    }

    private void OnDisable()
    {
        if (playerInteraction != null)
            playerInteraction.OnInteractableChanged -= HandleInteractableChanged;
    }

    private void Update()
    {
        if (playerInteraction == null) return;
        UpdateInteractionUI();
    }

    public void Initialized(PlayerInteraction playerInteraction)
    {
        this.playerInteraction = playerInteraction;
        string bindingText = playerInteraction.GameInput.InteractionBindingDisplay;
        keyText = $"[{bindingText}]";
        playerInteraction.OnInteractableChanged += HandleInteractableChanged;
    }

    private void UpdateInteractionUI()
    {
        if (currentInteractable == null || !currentInteractable.CanInteract || !currentInteractable.IsHoldInteract)
            return;

        if (interactionPanel.activeInHierarchy)
        {
            holdProgressFill.fillAmount = playerInteraction.GetHoldProgress();
        }
    }

    private void HandleInteractableChanged(IInteractable interactable)
    {
        currentInteractable = interactable;
        bool canShow = interactable != null && interactable.CanInteract;

        interactionPanel.SetActive(canShow);
        if (!canShow)
        {
            //holdProgressFill.fillAmount = 0;
            return;
        }

        holdProgressFill.fillAmount = 0;
        interactionText.text = interactable.IsHoldInteract
            ? $"Hold {keyText} {interactable.PromptName}"
            : $"{keyText} {interactable.PromptName}";
    }
}

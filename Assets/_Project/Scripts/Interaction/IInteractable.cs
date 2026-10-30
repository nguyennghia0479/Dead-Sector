using UnityEngine;

public interface IInteractable
{
    public string PromptName { get; }
    public bool CanInteract { get; }
    public bool IsHoldInteract { get; }
    public float HoldDuration { get; }

    public void SetHighlight(bool isHighlighted);
    public void OnInteract(PlayerController player);
}

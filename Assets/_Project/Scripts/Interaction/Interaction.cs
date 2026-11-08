using UnityEngine;

public class Interaction : MonoBehaviour, IInteractable
{
    [Header("Interact Settings")]
    [SerializeField] protected string pickupName;
    [SerializeField] protected bool canInteract = true;
    [SerializeField] protected bool isHoldInteract;
    [SerializeField] protected float holdDuration = 2f;
    [SerializeField] protected Material highlightMat;

    protected Renderer[] gunRenderers;
    protected Material[][] cachedOriginalMats;
    protected Material[][] cachedHighlightMats;

    public string PromptName => pickupName;

    public bool CanInteract => canInteract;

    public bool IsHoldInteract => isHoldInteract;

    public float HoldDuration => holdDuration;

    protected virtual void Awake()
    {
        gunRenderers = GetComponentsInChildren<Renderer>();
        cachedOriginalMats = new Material[gunRenderers.Length][];
        cachedHighlightMats = new Material[gunRenderers.Length][];

        for (int i = 0; i < gunRenderers.Length; i++)
        {
            cachedOriginalMats[i] = gunRenderers[i].sharedMaterials;

            int submeshCount = gunRenderers[i].sharedMaterials.Length;
            Material[] highlighArray = new Material[submeshCount];
            for (int j = 0; j < highlighArray.Length; j++)
                highlighArray[j] = highlightMat;

            cachedHighlightMats[i] = highlighArray;
        }
    }

    public void SetHighlight(bool isHighlighted)
    {
        for (int i = 0; i < gunRenderers.Length; i++)
        {
            gunRenderers[i].sharedMaterials = isHighlighted
                ? cachedHighlightMats[i]
                : cachedOriginalMats[i];
        }
    }

    public virtual void OnInteract(PlayerController player)
    {
        
    }
}

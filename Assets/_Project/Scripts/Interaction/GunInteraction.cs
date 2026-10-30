using UnityEngine;

public class GunInteraction : MonoBehaviour, IInteractable
{
    [Header("Interact Settings")]
    [SerializeField] private GunController gunPrefab;
    [SerializeField] private string pickupName;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool isHoldInteract;
    [SerializeField] private float holdDuration = 2f;
    [SerializeField] private Material highlightMat;

    private Renderer[] gunRenderers;
    private Material[][] cachedOriginalMats;
    private Material[][] cachedHighlightMats;

    public string PromptName => pickupName;

    public bool CanInteract => canInteract;

    public bool IsHoldInteract => isHoldInteract;

    public float HoldDuration => holdDuration;

    private void Awake()
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

    public void OnInteract(PlayerController player)
    {
        player.WeaponManager.PickupGun(gunPrefab, transform);
        Destroy(gameObject);
    }
}

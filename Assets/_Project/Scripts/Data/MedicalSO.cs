using UnityEngine;

[CreateAssetMenu(fileName = "MedicalSO", menuName = "Scriptable Objects/MedicalSO")]
public class MedicalSO : ItemSO
{
    [Header("Medical Info")]
    [SerializeField] private string medicalName;
    [SerializeField] private Sprite medicalIcon;
    [SerializeField] private MedicalCrateInteraction pickupPrefab;

    [Space]
    [Range(0f, 1f)]
    [SerializeField] private float healPercent = .5f;

    public string MedicalName => medicalName;
    public Sprite MedicalIcon => medicalIcon;
    public MedicalCrateInteraction PickupPrefab => pickupPrefab;
    public float HealPercent => healPercent;
}

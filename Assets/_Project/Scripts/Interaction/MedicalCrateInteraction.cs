using UnityEngine;

public class MedicalCrateInteraction : Interaction
{
    [SerializeField] private MedicalSO medicalSO;

    public override void OnInteract(PlayerController player)
    {
        GameEvents.RaiseMedicalUsed(medicalSO.HealPercent);
        Destroy(gameObject);
    }
}

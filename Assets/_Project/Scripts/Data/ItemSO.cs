using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public abstract class ItemSO : ScriptableObject
{
    [Header("Drop Chance Settings")]
    [Range(0f, 100f)]
    [SerializeField] protected float baseChance;
    [Range(0f, 1f)]
    [SerializeField] protected float chanceIncreasePerWave;
    [Range(0f, 100f)]
    [SerializeField] protected float maxChance;
    [Range(0f, 100f)]
    [SerializeField] protected float dropWeight;
    public float DropWeight => dropWeight;

    public float GetCurrentChance(int currentWave)
    {
        float currentChance = baseChance + chanceIncreasePerWave * (currentWave - 1);

        return Mathf.Min(currentChance, maxChance);
    }
}

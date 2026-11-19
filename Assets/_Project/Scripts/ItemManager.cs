using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private ItemDatabaseSO itemDatabase;
    [SerializeField] private int wavesPerTier = 5;
    [SerializeField] private int maxDropCount = 5;
    [SerializeField] private ItemSpawnPoint itemSpawnPoint;
    [SerializeField] private ItemDatabaseSO defaultItemDB;
    [SerializeField] private List<ItemSO> itemList;
    [SerializeField] private List<Sprite> droppedItems;

    private void Awake()
    {
        itemList = new List<ItemSO>(itemDatabase.items);
        itemList.Sort((a,b) => b.DropWeight.CompareTo(a.DropWeight));
    }

    private void OnEnable()
    {
        GameEvents.OnWaveCompleted += HandleWaveCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnWaveCompleted -= HandleWaveCompleted;
    }

    private void Start()
    {
        itemSpawnPoint = FindAnyObjectByType<ItemSpawnPoint>();
    }

    private void HandleWaveCompleted(int currentWave)
    {
        int dropCount = CalculateDropCount(currentWave);
        droppedItems.Clear();

        for (int i = 0; i < dropCount; i++)
            DropRandomItem(currentWave);

        if (droppedItems.Count == 0)
        {
            ItemSO item = defaultItemDB.items[Random.Range(0, defaultItemDB.items.Count)];
            DropItem(item);
        }

        GameEvents.RaiseItemsDropped(droppedItems);
    }

    private int CalculateDropCount(int currentWave)
    {
        int dropTier = (currentWave - 1) / wavesPerTier;
        int minDrop = Mathf.Min(1 + dropTier, maxDropCount);
        int maxDrop = Mathf.Min(2 + dropTier, maxDropCount);

        return Random.Range(minDrop, maxDrop + 1);
    }

    private void DropRandomItem(int currentWave)
    {
        foreach (var item in itemList)
        {
            float currentChance = item.GetCurrentChance(currentWave);
            if (Random.Range(0, 100) <= currentChance)
            {
                DropItem(item);
                break;
            }
        }
    }

    private void DropItem(ItemSO item)
    {
        Vector3 spawnPosition = itemSpawnPoint.GetRandomSpawnPosition();
        if (item is GunSO gun)
        {
            Instantiate(gun.PickupPrefab, spawnPosition, Quaternion.identity);
            droppedItems.Add(gun.GunIcon);
        }
        else if (item is ThrowableSO throwable)
        {
            Instantiate(throwable.PickupPrefab, spawnPosition, Quaternion.identity);
            droppedItems.Add(throwable.ThrowableIcon);
        }
        else if (item is MedicalSO medical)
        {
            Instantiate(medical.PickupPrefab, spawnPosition, Quaternion.identity);
            droppedItems.Add(medical.MedicalIcon);
        }
    }
}

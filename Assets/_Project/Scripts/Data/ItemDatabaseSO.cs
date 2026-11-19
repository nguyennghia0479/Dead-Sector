using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabaseSO", menuName = "Scriptable Objects/ItemDatabaseSO")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items;
}

using System.Collections.Generic;
using UnityEngine;

public class ThrowableController : MonoBehaviour
{
    [SerializeField] private List<ThrowableData> throwableList;

    private ThrowableData currentThrowable;
    private float throwForce;
    private Transform throwSpawn;

    public void Initialized(float throwForce, Transform throwSpawn)
    {
        this.throwForce = throwForce;
        this.throwSpawn = throwSpawn;
    }

    public void PicupThrowable(Throwable throwable, int amount)
    {
        foreach (var throwableData in throwableList)
        {
            if (throwableData.throwablePrefab.ThrowableType == throwable.ThrowableType)
            {
                throwableData.amount += amount;
                throwableData.amount = Mathf.Clamp(throwableData.amount, 0, throwable.MaxAmount);
                UpdateThrowableAmountUI(throwableData);
                return;
            }
        }

        ThrowableData newThrowable = new()
        {
            throwablePrefab = throwable,
            amount = amount
        };
        throwableList.Add(newThrowable);
        UpdateThrowableAmountUI(newThrowable);
    }

    public bool TryToThrow(ThrowableType throwableType)
    {
        foreach (var item in throwableList)
        {
            if (item.throwablePrefab.ThrowableType == throwableType)
                currentThrowable = item;
        }

        return currentThrowable != null && currentThrowable.amount > 0;
    }

    public void HandleThrown()
    {
        currentThrowable.amount--;
        UpdateThrowableAmountUI(currentThrowable);

        Throwable newThrowable = Instantiate(currentThrowable.throwablePrefab, throwSpawn.position, Quaternion.identity);
        newThrowable.GetComponent<Rigidbody>().AddForce(throwForce * Camera.main.transform.forward, ForceMode.Impulse);
        newThrowable.HandleBeThorwn();
    }

    public void HandleThrowFinished()
    {
        if (currentThrowable.amount <= 0)
            throwableList.Remove(currentThrowable);

        currentThrowable = null;
        gameObject.SetActive(false);
        GunController gun = GetComponentInParent<PlayerController>().GunActivated;
        gun.gameObject.SetActive(true);
    }

    private void UpdateThrowableAmountUI(ThrowableData throwableData)
    {
        GameEvents.RaiseThrowableAmountChanged(throwableData.throwablePrefab.ThrowableType, throwableData.amount);
    }
}

[System.Serializable]
public class ThrowableData
{
    public Throwable throwablePrefab;
    public int amount;
}

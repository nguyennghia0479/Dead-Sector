using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GunController[] slots;
    [SerializeField] private float switchTime = .5f;
    [SerializeField] private GunController activeSlot;

    private PlayerController player;
    private GameInput gameInput;
    private WaitForSeconds waitTime;
    private bool isSwitching;
    private int currentIndex;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        gameInput = GetComponent<GameInput>();
        waitTime = new WaitForSeconds(switchTime);
        currentIndex = 0;
    }

    private void OnEnable()
    {
        gameInput.OnSlotSelected += HandleSlotSelected;
    }

    private void OnDisable()
    {
        gameInput.OnSlotSelected -= HandleSlotSelected;
    }

    private void Start()
    {
        EquipItem(currentIndex);
    }

    public void PickupGun(GunController gun, Transform pickupTransform, int currentAmmos, int totalAmmos)
    {
        GunController newGun;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != activeSlot && slots[i] == null)
            {
                newGun = Instantiate(gun, player.GunHolder);
                slots[i] = newGun;
                slots[i].gameObject.SetActive(false);
                StartCoroutine(SwitchSlotRoutine(i));
                return;
            }
        }

        DropActiveGun(pickupTransform);
        newGun = Instantiate(gun, player.GunHolder);
        newGun.SetAmmos(currentAmmos, totalAmmos);
        activeSlot = newGun;
        slots[currentIndex] = activeSlot;
        StartCoroutine(SwitchSlotRoutine(currentIndex));
    }

    private void DropActiveGun(Transform gunTransform)
    {
        GunController dropGun = activeSlot;
        if (dropGun != null)
        {
            dropGun.SetGunAiming(false);
            GunInteraction newGunPickup = Instantiate(dropGun.GunDropped, gunTransform.position + Vector3.up, gunTransform.rotation);
            newGunPickup.SetAmmos(dropGun.CurrentAmmos, dropGun.TotalAmmos);
            Destroy(dropGun.gameObject, .1f);
        }
    }

    public void PickupThrowable(Throwable throwable, int amount)
    {
        player.ThrowableActivated.PicupThrowable(throwable, amount);
    }

    private void EquipItem(int index)
    {
        foreach (var slot in slots)
        {
            if (slot != null)
                slot.gameObject.SetActive(false);
        }

        activeSlot = slots[index];
        if (activeSlot.TryGetComponent<GunController>(out var gun))
            player.AssginGun(gun);
        activeSlot.gameObject.SetActive(true);

        UpdateGunsUI();
    }

    private IEnumerator SwitchSlotRoutine(int index)
    {
        isSwitching = true;
        if (activeSlot.TryGetComponent<GunController>(out var gun))
        {
            gun.PlayUnequipAnimation();
        }

        yield return waitTime;
        currentIndex = index;
        EquipItem(currentIndex);
        isSwitching = false;
    }

    private void UpdateGunsUI()
    {
        GunController primaryGun = slots[currentIndex];
        GunController secondaryGun = slots[1 - currentIndex];

        Sprite primaryIcon = primaryGun != null ? primaryGun.GunIcon : null;
        Sprite secondaryIcon = secondaryGun != null ? secondaryGun.GunIcon : null;
        GameEvents.RaiseWeaponSwitched(primaryIcon, secondaryIcon);
    }

    private void HandleSlotSelected(int index)
    {
        if (!isSwitching && currentIndex != index && slots[index] != null)
            StartCoroutine(SwitchSlotRoutine(index));
    }
}

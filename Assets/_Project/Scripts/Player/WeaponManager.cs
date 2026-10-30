using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject[] slots;
    [SerializeField] private float switchTime = .5f;
    [SerializeField] private GameObject activeSlot;

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

    public void PickupGun(GunController gun, Transform pickupTransform)
    {
        GunController newGun;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != activeSlot && slots[i] == null)
            {
                newGun = Instantiate(gun, player.GunHolder);
                slots[i] = newGun.gameObject;
                slots[i].SetActive(false);
                StartCoroutine(SwitchSlotRoutine(i));
                return;
            }
        }

        DropActiveGun(pickupTransform);
        newGun = Instantiate(gun, player.GunHolder);
        activeSlot = newGun.gameObject;
        slots[currentIndex] = activeSlot;
        StartCoroutine(SwitchSlotRoutine(currentIndex));
    }

    private void DropActiveGun(Transform gunTransform)
    {
        GameObject dropGun = activeSlot;
        if (dropGun.TryGetComponent<GunController>(out var gun))
        {
            Instantiate(gun.GunDropped, gunTransform.position, gunTransform.rotation);
            Destroy(dropGun);
        }
    }

    private void EquipItem(int index)
    {
        foreach (var slot in slots)
        {
            if (slot != null)
                slot.SetActive(false);
        }

        activeSlot = slots[index];
        activeSlot.SetActive(true);
        if (activeSlot.TryGetComponent<GunController>(out var gun))
            player.AssginGun(gun);
    }

    private IEnumerator SwitchSlotRoutine(int index)
    {
        isSwitching = true;
        if (activeSlot.TryGetComponent<GunController>(out var gun))
            gun.PlayUnequipAnimation();

        yield return waitTime;
        currentIndex = index;
        EquipItem(currentIndex);
        isSwitching = false;
    }

    private void HandleSlotSelected(int index)
    {
        if (!isSwitching && currentIndex != index && slots[index] != null)
            StartCoroutine(SwitchSlotRoutine(index));
    }
}

using System.Collections;
using UnityEngine;

public class SwitchManager : MonoBehaviour
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

    private void EquipItem(int index)
    {
        foreach (var slot in slots)
            slot.SetActive(false);

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
        if (!isSwitching && currentIndex != index)
            StartCoroutine(SwitchSlotRoutine(index));
    }
}

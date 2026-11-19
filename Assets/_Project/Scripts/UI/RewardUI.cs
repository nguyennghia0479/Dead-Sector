using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetupRewardUI(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}

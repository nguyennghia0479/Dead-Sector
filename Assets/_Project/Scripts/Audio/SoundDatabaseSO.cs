using UnityEngine;

public enum SoundType
{
    ButtonClicked, PickupItem, ZombieScream, ConcreteHit, MetalHit, DirtHit, HumanHit, WoodHit
}

[CreateAssetMenu(fileName = "SoundDatabaseSO", menuName = "Scriptable Objects/SoundDatabaseSO")]
public class SoundDatabaseSO : ScriptableObject
{
    [SerializeField] private SoundData[] soundList;

    public AudioClip GetRandomClipByType(SoundType type)
    {
        foreach (var soundData in soundList)
        {
            if (soundData.clips == null || soundData.clips.Length == 0)
                continue;

            if (soundData.soundType == type)
            {
                int randomIndex = Random.Range(0, soundData.clips.Length);
                return soundData.clips[randomIndex];
            }
        }

        return null;
    }
}

[System.Serializable]
public struct SoundData
{
    public SoundType soundType;
    public AudioClip[] clips;
}

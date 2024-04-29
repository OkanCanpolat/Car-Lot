using UnityEngine;
[CreateAssetMenu (fileName = "Save Data")]

public class SaveData : ScriptableObject
{
    public int CurrentGold;
    public int CurrentLevelIndex;
    public int MaxLevelIndex;
}

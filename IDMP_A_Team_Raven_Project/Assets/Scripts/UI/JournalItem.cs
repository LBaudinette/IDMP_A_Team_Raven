using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]
[CreateAssetMenu(fileName = "New journal Item", menuName = "Journal/Journal Entry")]
public class JournalItem : ScriptableObject
{
    public string monsterName;
    public string monsterDescription;
    public Sprite monsterImage;
    public int entryNumber;
}

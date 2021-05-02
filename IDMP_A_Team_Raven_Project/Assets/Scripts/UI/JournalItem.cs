using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]
[CreateAssetMenu(fileName = "New journal Item", menuName = "Journal/Journal Entry")]
public class JournalItem : ScriptableObject
{
    public string monsterName;

    [TextArea(15, 20)]
    public string monsterDescription;
    public Sprite monsterImage;
    public int entryNumber;
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JournalSlot : MonoBehaviour
{
    [Header("UI variables to change")]
    [SerializeField] private TextMeshProUGUI itemName;
    //[SerializeField] private Image monsterImage;

    [Header("Variables from the monster")]
    public JournalItem currentMonster;
    public JournalManager journalManager;

    public void Setup(JournalItem newMonster, JournalManager newMananger)
    {
        currentMonster = newMonster;
        journalManager = newMananger;
        if (currentMonster)
        {
            itemName.text = currentMonster.name;
            //monsterImage.sprite = currentMonster.monsterImage;
        }
    }

    public void ClickedOn()
    {
        if (currentMonster)
        {
            journalManager.SetupDescriptionAndSprite(currentMonster.monsterDescription, currentMonster);
        }
    }
}

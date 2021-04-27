using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
[CreateAssetMenu(fileName = "New Journal", menuName = "Journal/Player Journal")]
public class PlayerJournal : ScriptableObject
{
    public List<JournalItem> playerJournal = new List<JournalItem>();

    public void AddMonster(JournalItem itemToAdd)
    {
        if (!playerJournal.Contains(itemToAdd))
        {
            playerJournal.Add(itemToAdd);
        }
    }
}


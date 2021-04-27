using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class JournalManager : MonoBehaviour
{
    [Header("Journal Information")]
    public PlayerJournal playerJournal;

    [SerializeField] private GameObject blankJournalSlot;
    [SerializeField] private GameObject journalContentPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image monsterSprite;
    [SerializeField] private Image monsterSpriteContainer;
    public JournalItem currentMonster;

    private void SetDescriptionTextAndSprite(string description)
    {
        descriptionText.text = description;
        Color containerAlpha = monsterSpriteContainer.color;
        Color itemAlpha = monsterSprite.color;
        containerAlpha.a = 0f;
        itemAlpha.a = 0f;
        monsterSpriteContainer.color = containerAlpha;
        this.monsterSprite.color = itemAlpha;
    }

    void MakeJournalSlots()
    {
        if (playerJournal)
        {
            for (int i = 0; i < playerJournal.playerJournal.Count; i++)
            {
                GameObject temporaryJournalSlot = Instantiate(blankJournalSlot, journalContentPanel.transform.position, Quaternion.identity, journalContentPanel.transform);
                JournalSlot newSlot = temporaryJournalSlot.GetComponent<JournalSlot>();
                if (newSlot)
                {
                    newSlot.Setup(playerJournal.playerJournal[i], this);
                }
            }
        }
    }

    private void OnEnable()
    {
        ClearJournalSlots();
        MakeJournalSlots();
        SetDescriptionTextAndSprite("");
    }

    public void SetupDescriptionAndSprite(string newDescriptionString, JournalItem newItem)
    {
        currentMonster = newItem;
        descriptionText.text = newDescriptionString;
        Color containerAlpha = monsterSpriteContainer.color;
        Color itemAlpha = this.monsterSprite.color;
        containerAlpha.a = 1f;
        itemAlpha.a = 1f;
        monsterSpriteContainer.color = containerAlpha;
        this.monsterSprite.color = itemAlpha;
        this.monsterSprite.sprite = newItem.monsterImage;
    }

    private void ClearJournalSlots()
    {
        for (int i = 0; i < journalContentPanel.transform.childCount; i++)
        {
            Destroy(journalContentPanel.transform.GetChild(i).gameObject);
        }
    }
}

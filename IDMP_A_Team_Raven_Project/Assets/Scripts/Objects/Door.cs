﻿using UnityEngine;

public enum DoorType
{
    key,
    enemy,
    button
}

public class Door : Interactable
{
    [Header("Door Variables")]
    public DoorType thisDoorType;
    public bool open = false;
    public PlayerInventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;
    public BoxCollider2D triggerCollider;

    public void Open()
    {
        // Turn off the door's sprite renderer
        doorSprite.enabled = false;
        // set open to true
        open = true;
        // turn off the door's box collider
        physicsCollider.enabled = false;
        //turn off the door's trigger collider
        triggerCollider.enabled = false;
    }

    public void Close()
    {
        doorSprite.enabled = true;
        open = false;
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;
    }
}

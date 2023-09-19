using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public int itemID;
    public string description;
    public Sprite icon;

}

public enum ItemType
{
    SKIN,
    CURRENCY,
    POWERUP
}

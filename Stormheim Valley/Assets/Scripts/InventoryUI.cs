using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public GameObject slotPrefab;
    public InventorySlot[] slots;
    [SerializeField] private GameObject slotContainer;

    /// <summary>
    /// Sets up the inventory UI
    /// </summary>
    /// <param name="inventory"></param>
    public void Setup(Inventory inventory)
    { 
        slots = new InventorySlot[inventory.size];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = Instantiate(slotPrefab, slotContainer.transform).GetComponent<InventorySlot>();
            slots[i].item = inventory.GetItem(i);
        }
        Debug.Log("inventory setup");
    }

    /// <summary>
    /// Updates the UI to add an item to a given slot. Should only be called from the inventory, to ensure consistency.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    public void AddItem(Item item, int index)
    {
        slots[index].AddItem(item);
    }

    /// <summary>
    /// Updates the inventory UI to empty the slot. Should only be called from the inventory, to ensure consistency.
    /// </summary>
    /// <param name="index"></param>
    public void RemoveItem(int index)
    {
        slots[index].RemoveItem();
    }

    /// <summary>
    /// Used to switch the items in the UI with another item. Should only be called from the inventory, to ensure consistency.
    /// </summary>
    /// <param name="formerIndex"></param>
    /// <param name="newIndex"></param>
    public void ReplaceItem(int formerIndex, int newIndex)
    {
        Item item = slots[formerIndex].item;

        slots[formerIndex].item = slots[newIndex].item;
        slots[newIndex].item = item;
        slots[formerIndex].UpdateIcon();
        slots[newIndex].UpdateIcon();
    }
}

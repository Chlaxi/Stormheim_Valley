using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    
    public int size = 16;

    [SerializeField]
    private Item[] inventory;

    public Item testItem;

    public InventoryUI UI;
    private bool UIState = false;
    private void Start()
    {
        inventory = new Item[size];

        if (UI == null)
        {
            throw new System.Exception("No inventory UI specified");
        }
        UI.Setup(this);
        UIState = UI.gameObject.activeSelf;
    }

    private void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            UIState = !UIState;
            UI.gameObject.SetActive(UIState);
        }

        if (Input.GetKeyDown("q"))
        {
            Debug.Log(AddItem(testItem));
        }

        if (Input.GetKeyDown("e"))
        {
            Debug.Log(RemoveItem(0));
        }

        if (Input.GetKeyDown("r"))
        {
            Debug.Log("Replacing");
            ReplaceItem(0, 1);
        }
    }


    //Add Item
    public bool AddItem(Item item)
    {
        //Try to find current stack of similar item
        //int quantityLeft = item.quantity;
        int availableIndex = -1;
        for (int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i]==null)
            {
                if (availableIndex == -1)
                {
                    Debug.Log("available space found at " + i);
                    availableIndex = i;
                }
            }
            else if (inventory[i].name.Equals(item.name)){
                Debug.Log("Would add to stack at index "+i);
                //Add to stack.
                //subtract quantity left
                
                //if(quantityLeft==0)
                    //return true;
                
            }
        }

        if(availableIndex == -1)
        {
            Debug.Log("Inventory is full or there's no available space for this item");
            return false;
        }

        inventory[availableIndex] = item;
        Debug.Log("Added to " + availableIndex);
        UI.AddItem(item,availableIndex);
        return true;
    }

    public void Use(int index)
    {
        //Use Item through the item's use option
    }

    public bool RemoveItem(int index)
    {
        if(index < 0 || index >= inventory.Length)
        {
            Debug.Log("Index (" + index + ") is out of bounds");
            return false;
        }
        if(inventory[index] == null)
        {
            Debug.Log("No item found at index " + index);
            return false;
        }

        //Call "Remove" function for item.
        Debug.Log(inventory[index].name + " at index " + index + " was removed");
        inventory[index].DropItem(transform);
        inventory[index] = null;
        UI.RemoveItem(index);
        return true;
    }

    /// <summary>
    /// Gets the item stored in a certain index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Item GetItem(int index)
    {
        if(index<0 || index >= inventory.Length)
            return null;

        return inventory[index];
    }

    //Replace Item
    public void ReplaceItem(int formerIndex, int newIndex)
    {
        Item item = inventory[formerIndex];

        inventory[formerIndex] = inventory[newIndex];
        inventory[newIndex] = item;
        UI.ReplaceItem(formerIndex, newIndex);
    }

}

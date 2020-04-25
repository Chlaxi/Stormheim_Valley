using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int size = 16;

    [SerializeField]
    private Item[] inventory;

    public Item testItem;

    private void Start()
    {
        inventory = new Item[size];
    }

    private void Update()
    {
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
        return true;
    }

    //Replace Item
    public void ReplaceItem(int formerIndex, int newIndex)
    {
        Item item = inventory[formerIndex];

        inventory[formerIndex] = inventory[newIndex];
        inventory[newIndex] = item;
    }

}

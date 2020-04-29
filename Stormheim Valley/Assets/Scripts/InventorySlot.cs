using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    //public int quantity;
    [SerializeField] private Image iconSlot;
    
    public Item item;
    [SerializeField] private Sprite icon;

    public void OpenItemMenu()
    {
        //Open the dropdownmenu, where you can choose an action.
    }

    public void RemoveItem()
    {
        if (item == null)
        {
            return;
        }

        item = null;
        icon = null;
        UpdateIcon();
    }

    public void UseItem()
    {
        if (item == null)
            return;

        item.UseItem();
        RemoveItem();

     /*   if (quantity > 0)
        {
            quantity--;
            //Use
            if(quantity<=0){
                RemoveItem();
            }
        }
        else
        {
            Debug.Log("No items left. Why's it still here?!");
        }*/
    }

    public void AddItem(Item item) //, int quantity=1)
    {
        if (item != null)
        {
            this.item = item;
            icon = item.icon;
            UpdateIcon();
        }
        //this.quantity += quantity;
    }

    public void UpdateIcon()
    {
        icon = (item==null) ? null : item.icon;

        iconSlot.sprite = icon;
        if (icon == null)
        {
            iconSlot.color = new Color(0,0,0,0);
        }
        else
        {
            iconSlot.color = new Color(255, 255, 255, 255);
        }
    }

}

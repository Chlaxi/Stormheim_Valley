using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteractable : Interactable
{
    public Item item;

    public override void Interact()
    {
        PickUp();
        base.Interact();
        
    }

    public void PickUp()
    {
        Debug.Log("picked up " + item.name);
        if (player.GetComponent<Inventory>().AddItem(item))
        {
            Debug.Log("Item picked up!");
            Destroy(gameObject);
        }
        Debug.Log("Interaction failed");
    }

}

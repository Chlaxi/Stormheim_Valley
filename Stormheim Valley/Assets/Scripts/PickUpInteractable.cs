using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteractable : Interactable
{
    public Item item;

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    public void PickUp()
    {
        Debug.Log("picked up " + item.name);
        //Add item to inventory, if space is availble

        //If  pickUp successful: Destroy
        Destroy(gameObject);
    }
}

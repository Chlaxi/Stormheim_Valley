using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Items")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;

    [SerializeField]
    private GameObject drop;

    public void DropItem(Transform position)
    {
        if(drop == null)
        {
            Debug.LogWarning(name + " doesen't have any dropped item. Removed from inventory, but nothing dropped.");
            return;
        }
        Instantiate(drop, position.position, Quaternion.identity);;
    }

    public void UseItem()
    {
        //Implement
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : Interactable
{

    public GameObject item;

    Vector3 randomPos = new Vector3(0, 0, 0);


    // Start is called before the first frame update
    public override void Interact()
    {
        base.Interact();
        randomPos = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
        GameObject newItem = Instantiate(item,transform.position + randomPos, Quaternion.identity);
        //newItem.GetComponent<Rigidbody2D>().AddForce(randomPos);
        

    }

  
}

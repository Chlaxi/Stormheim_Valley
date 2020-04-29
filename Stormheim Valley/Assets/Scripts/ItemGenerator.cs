using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : Interactable
{

    public GameObject item;

    Vector3 randomPos = new Vector3(0, 0, 0);
    [Header("Generator settings")]
    [SerializeField] private bool generateAllAtOnce=false;
    [SerializeField] private int maxQuantity = 1;
    [SerializeField] private int availableQuantity = 1;

    [SerializeField][Tooltip("Generates a random amount of items between the two values (both are inclusive)")]
    Vector2Int itemsGenerated = new Vector2Int(1,1);
    
    [SerializeField] private int cooldown = 5;
    private float timeLeft;

    [SerializeField] private bool canHarvest = true;

    private void Start()
    {
        timeLeft = cooldown;
    }

    protected override void Update()
    {
        base.Update();

        //Handles the respawn of items
        if (availableQuantity < maxQuantity)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                availableQuantity += Random.Range(itemsGenerated.x, itemsGenerated.y+1);
                if (availableQuantity > maxQuantity)
                    availableQuantity = maxQuantity;
                timeLeft = cooldown;
            }
        }

    }

    public override bool InitiateInteraction(PlayerController player)
    {
        if (availableQuantity > 0)
        {
            return base.InitiateInteraction(player);
        }
        else
        {
            Debug.Log("No avaiable spawns");
            //Play "Interaction denied" anim/sound
            return false;
        }
    }

    // Start is called before the first frame update
    public override void Interact()
    {

        base.Interact();
        int quantity = 0;
        if (generateAllAtOnce)
        {
            quantity = availableQuantity;
            availableQuantity = 0;
        }
        else
        {
            if (availableQuantity > 0)
            {
                quantity = 1;
                availableQuantity--;
            }
        }

        for (int i = 0; i < quantity; i++)
        {
            randomPos = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
            GameObject newItem = Instantiate(item,transform.position + randomPos, Quaternion.identity);
            //newItem.GetComponent<Rigidbody2D>().AddForce(randomPos);
        }
        

    }

  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteracting = false;
    public float interactionTime=0;
    [SerializeField]
    private float _interactionTime = 0;
    [SerializeField]
    protected PlayerController player;
    public bool resetOnStop = false;

    private void Start()
    {
        _interactionTime = interactionTime;
    }

    private void Update()
    {
        if (isInteracting)
        {
            _interactionTime -= Time.deltaTime;
            //Draw the progress bar
            if (_interactionTime <= 0)
            {
                Interact();
            }
        }
    }


    public virtual void InitiateInteraction(PlayerController player)
    {
        this.player = player;
        isInteracting = true;
        Debug.Log("Initiating interaction with: " + name);
    }

    public virtual void Interact()
    {
        

        if (player != null)
            player.StopInteraction();

        StopInteraction();

        Debug.Log("Interacted with " + name);
        
    }

    public virtual void StopInteraction()
    {

   
        if (isInteracting)
        {
            player = null;

            if (resetOnStop)
            {
                ResetInteraction();
            }
        }
        isInteracting = false;

    }

    public virtual void ResetInteraction()
    {
        _interactionTime = interactionTime;
        Debug.Log("Interaction with " + name + "was reset");
    }
}

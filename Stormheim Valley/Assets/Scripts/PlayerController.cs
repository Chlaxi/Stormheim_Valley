using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private Interactable currentInteractable;



    [SerializeField]
    protected bool isMoving;
    public bool pointerMovement;
    public float moveDelay;

    private float _moveDelay;
    private bool followingPointer = false;

    [SerializeField]
    private float interactionRadius;

    public float speed = 2f;


    Vector2 dir;
    // public LayerMask layerMasks;

    [SerializeField]
    LayerMask interactableLayer;
    Rigidbody2D rg;
    Animator animator;

    [SerializeField]
    private Vector3 mousePosition;
    
    
    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _moveDelay = moveDelay;
    }

    void Update()
    {
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (pointerMovement && Input.GetMouseButton(0))
        {
            if (followingPointer)
            {
                FollowCursor();
            }
            else
            {
                PrepMouseMove();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Add pathfinding. Should be overwritten when holding

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100, interactableLayer);
            if (hit.collider != null)
            {
                Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
                if (interactable != null && Vector2.Distance(interactable.location.position,transform.position) <= interactionRadius)
                {
                    Interact(interactable);
                }
            }
        }

        if (pointerMovement && Input.GetMouseButtonUp(0))
        {
            followingPointer = false;
            _moveDelay = moveDelay;
        }
    }

    private void Interact(Interactable interactable)
    {
        currentInteractable = interactable;
        Vector2 direction = (interactable.transform.position - transform.position).normalized;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        interactable.InitiateInteraction(this);
        animator.SetBool("Interacting",true);
        
       
    }
    

    private void PrepMouseMove()
    {
        if (_moveDelay > 0f)
        {
            _moveDelay -= Time.deltaTime;
        }
        else
        {
            followingPointer = true;
        }
    }

    private void FollowCursor()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Update distance checking
        if (Vector2.Distance(mousePosition, rg.position)<0.5f)
        {
            Debug.Log("cursor on player");
            dir = new Vector2(0, 0);
            return;
        }

        dir = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y).normalized;


    }

    void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Moves the player based on the direction granted from either the pointer or controls
    /// </summary>
    void Move()
    {
        Vector2 velocity = dir * speed;
        rg.MovePosition(rg.position + velocity * Time.deltaTime);
        
        if(velocity.magnitude >= 0.1f){
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
            isMoving = true;

            StopInteraction();        
        }
        else
        {
            isMoving = false;

        }
        animator.SetFloat("Speed", velocity.magnitude);

    }

    public void StopInteraction()
    {
        animator.SetBool("Interacting", false);
        if (currentInteractable != null)
        {
            Debug.Log("----Stop interacting with " + currentInteractable.name);
            if(currentInteractable.isInteracting)
                currentInteractable.StopInteraction();
            
            currentInteractable = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Animator))]
public class PlayerController : MonoBehaviour
{
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100, interactableLayer);
            if (hit.collider != null)
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                //TODO Check distance
                if (interactable != null && Vector2.Distance(interactable.transform.position,transform.position) <= interactionRadius)
                {
                    //Look at interactable
                    interactable.Interact();
                }
            }
        }

        if(pointerMovement && Input.GetMouseButtonUp(0))
        {
            followingPointer = false;
            _moveDelay = moveDelay;
        }
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
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("Speed", velocity.magnitude);

        //TODO: Keeps animation face direction

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

}

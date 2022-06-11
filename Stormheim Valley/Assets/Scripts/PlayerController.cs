using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


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
    private bool UIHit = false;

    [SerializeField]
    private float interactionRadius;

    public float speed = 2f;

    private GraphicRaycaster graphicsRaycaster;
    private EventSystem eventSystem;

    Vector2 dir;
    Vector2 pointDir;
    // public LayerMask layerMasks;

    [SerializeField]
    LayerMask interactableLayer;
    Rigidbody2D rg;
    Animator animator;

    [SerializeField]
    private Vector3 mousePosition;
    
    
    private void Start()
    {
        graphicsRaycaster = GameObject.Find("MainCanvas").GetComponent<GraphicRaycaster>();

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _moveDelay = moveDelay;
    }

    void Update()
    {
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (pointerMovement)
        {
            PointerMove();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Action();
        }
        
    }

    private void PointerMove()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!followingPointer)
            {
                Action();
            }
            followingPointer = false;
            UIHit = false;
            _moveDelay = moveDelay;

        }

        if (UIHit)
            return;

        if (Input.GetMouseButton(0))
        {
            if (followingPointer)
            {
                dir = GetPointerDir();
                return;
            }

            PrepPointerMove();
        }
    }

    private void SetPointDir(Vector2 direction)
    {
        
        if (direction.magnitude != 0)
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            pointDir = direction.normalized;
        }
    }

    private void Action()
    {
        //TODO: Rewrite so it can be used by controller
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100, interactableLayer);
        SetPointDir(GetPointerDir());

        if (hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
            if (interactable != null && Vector2.Distance(interactable.location.position, transform.position) <= interactionRadius)
            {
                Interact(interactable);
                return;
            }
        }
        UseItem();
    }

    private void Interact(Interactable interactable)
    {
        currentInteractable = interactable;
        Vector2 direction = (interactable.transform.position - transform.position).normalized;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        if (interactable.InitiateInteraction(this))
        {
            //TODO: Make animation based on interactable object
            animator.SetBool("Interacting", true);
        }     
    }
    
    private void UseItem()
    {
        //Get equiped item and call its use function.
        //TODO: Make animation based on item used
        Debug.Log("Pew pew!");
    }

    private void PrepPointerMove()
    {
        //Set up the new Pointer Event
        PointerEventData m_PointerEventData = new PointerEventData(eventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        graphicsRaycaster.Raycast(m_PointerEventData, results);
        foreach (RaycastResult result in results)
        {
            Debug.Log("Hit " + result.gameObject.name + "SortingLayer="+ result.gameObject.layer);

            if (result.gameObject.layer == 5)
            {
                Debug.Log("UI hit. not prepping movement");
                followingPointer = false;
                UIHit = true;
                return;
            }
        }

        if (_moveDelay > 0f)
        {
            _moveDelay -= Time.deltaTime;
        }
        else
        {
            followingPointer = true;
        }
    }

    private Vector2 GetPointerDir()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        //Update distance checking
        if (Vector2.Distance(mousePosition, rg.position)<0.5f)
        {
            Debug.Log("cursor on player");
            return new Vector2(0, 0);
            
        }
        return new Vector2(
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
        SetPointDir(dir);
        if (velocity.magnitude >= 0.1f){
            //animator.SetFloat("Horizontal", dir.x);
            //animator.SetFloat("Vertical", dir.y);
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)pointDir, new Vector3(1,1,1));
    }
}

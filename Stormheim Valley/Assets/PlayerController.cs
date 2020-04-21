using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public bool pointerMovement;
    public float speed = 2f;

    Vector2 dir;
   // public LayerMask layerMasks;


    Rigidbody2D rg;
    Animator animator;

    [SerializeField]
    private Vector3 mousePosition;
    
    
    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (pointerMovement && Input.GetMouseButton(0))
        {
            FaceCursor();
        }
        else
        {

            dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }
    }


    private void FaceCursor()
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
        Vector2 velocity = dir * speed;
        rg.MovePosition(rg.position + velocity * Time.deltaTime);
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("Speed", velocity.magnitude);
        
        //TODO: Keeps animation face direction

    }

}

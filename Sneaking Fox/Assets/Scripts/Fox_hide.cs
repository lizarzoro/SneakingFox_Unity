using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_hide : MonoBehaviour
{
    private Rigidbody2D rb;
    private float dirX;
    private float moveSpeed = 10f;
    private SpriteRenderer rend;
    private bool canHide = false;
    private bool hiding = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        
        if(canHide && Input.GetKey("up"))
        {
            Physics2D.IgnoreLayerCollision(10, 11, true);
            rend.sortingOrder = 0;
            hiding = true;
        }

        else
        {
            Physics2D.IgnoreLayerCollision(10, 11, false);
            rend.sortingOrder = 2;
            hiding = false;
        }
    }
    private void FixedUpdate()
    {
        if (!hiding)
            rb.velocity = new Vector2(dirX, rb.velocity.y);
        else
            rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("bush"))
        {
            canHide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("bush"))
        {
            canHide = false;
        }   
    }
}

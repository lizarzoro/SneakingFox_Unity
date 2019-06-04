using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_hide : MonoBehaviour
{
    private float dirX;
    private float moveSpeed = 7f;
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject gotchaText;

    private void Start()
    {
        gotchaText.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
    }
    private void Update()
    {
        if (transform.position.x < -9f)
            dirX = 1f;
        else if (transform.position.x > 9f)
            dirX = -1f;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Player (1)"))
            gotchaText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Player (1)"))
            gotchaText.SetActive(false);
    }
}

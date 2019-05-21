using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Ladder : MonoBehaviour
{
    public float speed, jumpForce, cooldownHit;
    public bool running, up, down, jumping, crouching, dead, attacking, special, isClimbing;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sp;
    private float rateOfHit;
    private GameObject[] life;
    private int qtdLife;
    private float inputHorizontal, inputVertical;
    public float distance; // 사다리 거리
    public LayerMask whatIsLadder;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputHorizontal * speed, rb.velocity.y);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);

        // 사다리 올라가는 상태인지 확인
        if (hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isClimbing = true;
                Debug.Log("isClimbing = true");
            }
            
            
        }
        else
        {
            isClimbing = false;
        }

        //
        if (isClimbing == true)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * 5);
            rb.gravityScale = 0;
            Debug.Log("gravityScale = 0");
        }
        else
        {
            rb.gravityScale = 2;
        }
    }


}

using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Move : MonoBehaviour
{
    public C_Controller controller;

    private Rigidbody2D rb;

    public float speed, jumpForce, cooldownHit;
    public float runSpeed = 40f;

    float inputHorizontal, inputVertical = 0f;
    //float horizontalMove = 0f;
    bool jump = false;
   
    bool crouch = false;

    public bool running, up, down, jumping, crouching, dead, attacking, special, isClimbing = false;

    // Anim
    private Animator anim;
    private SpriteRenderer sp;


    // 점프
    private float moveInput;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private int extraJumps;
    public int extraJumpsValue;

    // 사다리 타기
    public LayerMask whatIsLadder;
    public float distance;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //sp = GetComponent<SpriteRenderer>();
        sp = GetComponent<SpriteRenderer>();
        running = false;
        up = false;
        down = false;
        jumping = false;
        crouching = false;
        isClimbing = false;
        //rateOfHit = Time.time;
        //life = GameObject.FindGameObjectsWithTag("Life");
        //qtdLife = life.Length;
        extraJumps = extraJumpsValue;
    }
    void Update()
    {

        
        

        //if (Input.GetButtonDown("Jump"))
        //{
        //    jump = true;
        //}

        // double jump
        
        //KeyCode.X
 

        

    }

    void FixedUpdate()
    {
        jump = false;
        //moveInput = Input.GetAxis("Horizontal");
        //rb.velocity = new Vector2(inputHorizontal * speed, rb.velocity.y);

        if (dead == false)
        {
            //Character doesnt choose direction in Jump									//If you want to choose direction in jump
            if (attacking == false)
            {                                                   //just delete the (jumping==false)
                if (crouching == false)
                {

                    Movement();
                    //Attack();
                    //Special();
                }
                Jump();
                Crouch();
                Ladder();
            }
            //Dead();
        }

       
       

        
    }

    void Movement()
    {
        //move
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(inputHorizontal * runSpeed, rb.velocity.y);

        controller.Move(inputHorizontal * Time.fixedDeltaTime, crouch, jump);
        inputHorizontal = Input.GetAxisRaw("Horizontal") * runSpeed;

        //run
        if (Input.GetKey(KeyCode.Z) && !jumping)
        {
            //rb.velocity = new Vector2(inputHorizontal * speed * Time.deltaTime * 3, rb.velocity.y);
            runSpeed = 20f;
            running = true;
        }
        else
        {
            //rb.velocity = new Vector2(inputHorizontal * speed * Time.deltaTime, rb.velocity.y);
            runSpeed = 10f;
            running = false;
        }

        if (rb.velocity.x != 0 && running == false)
        {
            //anim.SetTrigger("Walking");
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
        if (rb.velocity.x != 0 && running == true)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            //jump = true;
            extraJumps--;
            //rb.AddForce(new Vector2(0, jumpForce));
        }
        else if (Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            //jump = true;
        }

        //Jump Animation
        if (rb.velocity.y > 0 && up == false)
        {
            up = true;
            jumping = true;
            anim.SetTrigger("Up");
        }
        else if (rb.velocity.y < 0 && down == false)
        {
            down = true;
            jumping = true;
            anim.SetTrigger("Down");
        }
        else if (rb.velocity.y == 0 && (up == true || down == true))
        {
            up = false;
            down = false;
            jumping = false;
            anim.SetTrigger("Ground");
        }
    }

    void Crouch()
    {
        //crouch
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            crouch = true;
            anim.SetBool("Crouching", true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            crouch = false;
            anim.SetBool("Crouching", false);
        }
    }

    void Ladder()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputHorizontal * speed, rb.velocity.y);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);

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
            Debug.Log("gravityScale = 2");
        }
    }
}

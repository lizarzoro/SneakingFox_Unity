using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class Fox_Move : MonoBehaviour
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
    

    // 사다리 타기
    public LayerMask whatIsLadder;
    public float distance;

    // 점프
    private float moveInput;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private int extraJumps;
    public int extraJumpsValue;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        running = false;
        up = false;
        down = false;
        jumping = false;
        crouching = false;
        isClimbing = false;
        rateOfHit = Time.time;
        life = GameObject.FindGameObjectsWithTag("Life");
        qtdLife = life.Length;
        extraJumps = extraJumpsValue;
    }

    void Update()
    {
        if(isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }
        //KeyCode.X
        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
            //rb.AddForce(new Vector2(0, jumpForce));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (dead == false)
        {
            //Character doesnt choose direction in Jump									//If you want to choose direction in jump
            if (attacking == false)
            {                                                   //just delete the (jumping==false)
                if (jumping == false && crouching == false)
                {
                    
                    Movement();
                    Attack();
                    Special();
                    Ladder();

                }
                Jump();
                Crouch();
            }
            Dead();
        }

    }

    void Ladder()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        //rb.velocity = new Vector2(inputHorizontal * speed, rb.velocity.y);
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

    void Movement()
    {
        
        //Character Move
        float move = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.Z))
        {
            //Run
            rb.velocity = new Vector2(move * speed * Time.deltaTime * 3, rb.velocity.y);
            running = true;
        }
        else
        {
            //Walk
            rb.velocity = new Vector2(move * speed * Time.deltaTime, rb.velocity.y);
            running = false;
        }

        //Turn
        if (rb.velocity.x < 0)
        {
            sp.flipX = true;
        }
        else if (rb.velocity.x > 0)
        {
            sp.flipX = false;
        }
        //Movement Animation
        if (rb.velocity.x != 0 && running == false)
        {
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
        //Jump
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded == true)
        {
            extraJumps = 2;
        }
        ////Jump Animation
        //if (rb.velocity.y > 0 && up == false)
        ////
        //{
        //    up = true;
        //    jumping = true;
        //    anim.SetTrigger("Up");
        //}
        //else if (rb.velocity.y < 0 && down == false)
        //    //
        //{
        //    down = true;
        //    jumping = true;
        //    anim.SetTrigger("Down");
        //}
        //else if (rb.velocity.y == 0 && (up == true || down == true)
        //{//)
        //    up = false;
        //    down = false;
        //    jumping = false;
        //    anim.SetTrigger("Ground");
        //}
    }

    void Attack()
    {                                                               //I activated the attack animation and when the 
                                                                    //Atacking																//animation finish the event calls the AttackEnd()
        if (Input.GetKeyDown(KeyCode.C))
        {
            rb.velocity = new Vector2(0, 0);
            anim.SetTrigger("Attack");
            attacking = true;
        }
    }

    void AttackEnd()
    {
        attacking = false;
    }

    void Special()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("Special", true);
        }
        else
        {
            anim.SetBool("Special", false);
        }
    }

    void Crouch()
    {
        //Crouch
        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetBool("Crouching", true);
        }
        else
        {
            anim.SetBool("Crouching", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {                           //Case of Bullet
        if (other.tag == "Enemy")
        {
            anim.SetTrigger("Damage");
            Hurt();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {                       //Case of Touch
        if (other.gameObject.tag == "Enemy")
        {
            anim.SetTrigger("Damage");
            Hurt();
        }
    }

    void Hurt()
    {
        if (rateOfHit < Time.time)
        {
            rateOfHit = Time.time + cooldownHit;
            Destroy(life[qtdLife - 1]);
            qtdLife -= 1;
        }
    }

    void Dead()
    {
        if (qtdLife <= 0)
        {
            anim.SetTrigger("Dead");
            dead = true;

        }
    }

    public void TryAgain()
    {                                                       //Just to Call the level again
        SceneManager.LoadScene(0);
    }
}

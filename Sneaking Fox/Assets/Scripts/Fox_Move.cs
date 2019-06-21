using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    float inputHorizontal, inputVertical = 0f;
    bool jump = false;
    bool crouch = false;

    // 이동2
    public C_Controller controller;
    public float runSpeed = 40f;
    public float MoveH = 0f;

    // 라이프
    public int health;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullheart;
    public Sprite emptyheart;

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

    // 숨기
    private bool canHide = false;
    public static bool hiding = false;

    // 경사로 애니메이션 오류 대응
    private bool onSlope = false;

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



        if (isGrounded == true)
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

        // 생명
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullheart;
            }
            else
            {
                hearts[i].sprite = emptyheart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;


            }
        }

        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        inputHorizontal = Input.GetAxisRaw("Horizontal") * speed;


        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    void FixedUpdate()
    {
        controller.Move(inputHorizontal * Time.fixedDeltaTime, crouch, jump);
        jump = false;


        if (dead == false)
        {
            //Character doesnt choose direction in Jump									
            //If you want to choose direction in jump
            if (attacking == false)
            {                                                   
                //just delete the (jumping==false)
                if (jumping == false && crouching == false)
                {
                    Movement();
                    Attack();
                    Special();
                    Hide();
                }
                Jump();
                Crouch();
                Ladder();
            }
            Dead();
        }

    }

    // 충돌 물체 확인
    void OnTriggerEnter2D(Collider2D other)
    {                           //Case of Bullet
        if (other.tag == "Enemy")
        {
            anim.SetTrigger("Damage");
            Hurt();
        }

        else if (other.tag == "bush")
        {
            canHide = true;
        }

        else if (other.tag == "slope")
        {
            onSlope = true;
        }
    }

    // 부쉬 찾기, 경사로 나올 때
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("bush"))
        {
            canHide = false;
        }

        else if (other.gameObject.tag.Equals("slope"))
        {
            onSlope = false;
        }
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //Run
            rb.velocity = new Vector2(inputHorizontal * speed * Time.deltaTime * 3, rb.velocity.y);
            running = true;
        }
        else
        {
            //Walk
            rb.velocity = new Vector2(inputHorizontal * speed * Time.deltaTime, rb.velocity.y);
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

    void Hide()
    {
        // 숨기
        if (canHide && Input.GetKey("down"))
        {
            Physics2D.IgnoreLayerCollision(10, 11, true);
            sp.sortingOrder = 1;
            hiding = true;
        }

        else
        {
            Physics2D.IgnoreLayerCollision(10, 11, false);
            sp.sortingOrder = 3;
            hiding = false;
        }
        // 숨은 상태에서 움직이기?
        if (!hiding)
            rb.velocity = new Vector2(inputHorizontal, rb.velocity.y);
        else
            rb.velocity = Vector2.zero;
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

    void Jump()
    {
        //Jump
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (isGrounded == true)
        {
            extraJumps = 2;
        }

        //Jump Animation
        if (rb.velocity.y > 0 && up == false && onSlope == false)
        {
            up = true;
            jumping = true;
            anim.SetTrigger("Up");
        }
        else if (rb.velocity.y < 0 && down == false && onSlope == false)
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

    void Attack()
    {                                                               //Atacking																//animation finish the event calls the AttackEnd()
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


    void OnCollisionEnter2D(Collision2D other)
    {                       //Case of Touch
        if (other.gameObject.tag == "Enemy")
        {
            //anim.SetTrigger("Damage");
            Hurt();
        }
    }

    void Hurt()
    {
        if (rateOfHit < Time.time)
        {
            rateOfHit = Time.time + cooldownHit;
            //Destroy(life[qtdLife - 1]);
            Destroy(hearts[numOfHearts - 1]);
            //qtdLife -= 1;
            numOfHearts -= 1;
        }
    }

    void Dead()
    {
        if (numOfHearts <= 0)
        {
            anim.SetTrigger("Dead");
            dead = true;
            SceneManager.LoadScene(0);
            // restart
            //Application.LoadLevel(Application.LoadLevel);
        }
    }

    public void TryAgain()
    {                                                       //Just to Call the level again
        SceneManager.LoadScene(0);
    }
}

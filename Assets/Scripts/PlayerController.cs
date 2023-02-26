using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;  // rigidbody
    private BoxCollider2D boxCollider2d; // boxcollider
    private Collision coll; // collision circles
    private SpriteRenderer rend;
    public enum form { parasite, gunner }
    public form currentForm;
    private enum AnimState { idle, walk, jump };
    private AnimState moveState;

    [Space]
    [Header("Base Game Stats")]
    public float walkSpeed;
    public float lungeHeight;
    public float lungeDash;


    private float moveInputX; // Either -1 (left), 1 (right), or 0 (no input)
    private float moveInputY; // Similiar to X
    private float playerDirection = 1; // -1 or 1, similar to moveInputX

    public bool isGrounded;
    public bool isLunging = false;

    [Space]
    // animation
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();

        moveState = AnimState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        // configures movement based on form
        if (currentForm == form.parasite)
        {
            // reconfigures speeds based on build
            walkSpeed = 10;
            lungeHeight = 15;
            lungeDash = 16;

            // resize & recolor
            //transform.localScale = new Vector3(0.5f, 0.3f);
            coll.bottomOffset = new Vector2(0.0f, -0.5f);
            rend.color = new Color(1.0f, 0.2117647f, 0.2117647f, 1.0f);
        }
        else if (currentForm == form.gunner)
        {
            walkSpeed = 14;
            lungeHeight = 20;
            lungeDash = 6;
            //transform.localScale = new Vector3(0.7f, 1.2f);
            //coll.bottomOffset = new Vector2(0.0f, -0.6f);
            rend.color = new Color(0.0f, 0.4433962f, 0.02627836f, 1.0f);
        }

        moveInputX = Input.GetAxis("Horizontal");
        moveInputY = Input.GetAxis("Vertical");

        // raw movement input (0 or 1)
        float rawX = Input.GetAxisRaw("Horizontal");
        float rawY = Input.GetAxisRaw("Vertical");

        playerDirection = (rawX != 0) ? rawX : playerDirection; // sets player direction based on input

        isGrounded = coll.onGround;

        Vector2 dir = new Vector2(rawX, moveInputY);

        if (!isLunging)
            Walk(dir);


        // jump/lunge
        if (Input.GetKeyDown(KeyCode.Space) && !isLunging && isGrounded)
        {
            Debug.Log("jump!");
            Lunge(dir);
        }
        
        anim.SetInteger("State", (int)moveState);
    }

    private void FixedUpdate()
    {
         // flip play in case of new move input
        if (moveInputX < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        } else if (moveInputX > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        /*
        */

        if (isLunging)
        {
            moveState = AnimState.jump;
        } else
        {
            if (moveInputX != 0 && !coll.onWall) // player is moving left
            {
                moveState = AnimState.walk;
            }
            else
            {
                moveState = AnimState.idle;
            }
        }
    }

    private void Walk(Vector2 dir)
    {
        rb.velocity = new Vector2(dir.x * walkSpeed, rb.velocity.y);
    }

    private void Lunge(Vector2 dir)
    {
        // boost horizontal movement and lose controls when in parasitic form
        if (currentForm == form.parasite)
        {
            StartCoroutine(LungeWait(0.7f));
        } else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * lungeHeight;
        }

    }

    // stops player controls for a second
    IEnumerator LungeWait(float time)
    {
        isLunging = true;

        yield return new WaitForSeconds(0.2f);

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * lungeHeight;

        rb.velocity += Vector2.right * playerDirection * lungeDash;
        if (moveInputX > 0.2f || moveInputX < -0.2f)
            rb.velocity = rb.velocity * new Vector2(0.9f, 1.0f);

        yield return new WaitForSeconds(time);
        isLunging = false;
    }
}

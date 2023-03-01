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
    public enum AnimState { idle, walk, jump, damage, transform, possessedIdle, possessedWalk, possessedJump };
    public AnimState moveState;

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
    public bool isTakingDamage = false;
    public bool transformIsFinished = true;
    public bool isTransformingToParasite = false;

    [Space]
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip skitter;
    public AudioClip scream;
    public AudioClip shoot;

    [Space]
    // animation
    private Animator anim;

    public GameObject gun;


    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();

        moveState = AnimState.idle;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentForm == form.parasite)
            gun.SetActive(false);
        else
            gun.SetActive(true);

        if (isTakingDamage)
            anim.SetInteger("State", (int)moveState);

        if (!isTakingDamage)
        {
            // configures movement based on form
            if (currentForm == form.parasite)
            {
                // reconfigures speeds based on build
                walkSpeed = 8;
                lungeHeight = 15;
                lungeDash = 14;
                transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
                coll.bottomOffset = new Vector2(0.0f, -0.5f);
                boxCollider2d.size = new Vector2(0.544f, 0.69f);
            }
            else if (currentForm == form.gunner)
            {
                walkSpeed = 6;
                lungeHeight = 28;
                lungeDash = 2;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                coll.bottomOffset = new Vector2(0.0f, -1.0f);
                boxCollider2d.size = new Vector2(0.544f, 1.68f);
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

            if (Input.GetKeyDown(KeyCode.E) && currentForm == form.gunner)
            {
                 changeToParasite(dir);
            }

            anim.SetInteger("State", (int)moveState);
        }
        else
        {
            StartCoroutine(DamageKnockback());
        }
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

        if (currentForm == form.parasite)
        {
            if (isTakingDamage)
            {
                moveState = AnimState.damage;
            }
            else
            {
                if (isLunging)
                {
                    moveState = AnimState.jump;
                }
                else
                {
                    if (moveInputX != 0 && !coll.onWall) // player is moving left
                    {
                        moveState = AnimState.walk;

                        if (!audioSource.isPlaying) // play skitter audio
                            audioSource.PlayOneShot(skitter);
                    }
                    else
                    {
                        moveState = AnimState.idle;
                    }
                }
            }
        } else if (transformIsFinished) // gunner
        {
            if (moveInputX == 0 && !coll.onWall && coll.onGround)
            {
                moveState = AnimState.possessedIdle;
            } else if (moveInputX != 0 && !coll.onWall && coll.onGround)
            {
                moveState = AnimState.possessedWalk;
            } else
            {
                moveState = AnimState.possessedJump;
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
            audioSource.PlayOneShot(scream);
            StartCoroutine(LungeWait(0.4f));
        } else
        {
            StartCoroutine(JumpWait(0.2f));
        }

    }

    public void changeToGunner()
    {
        transform.position += new Vector3(0.0f, 0.34f, 0.0f);

        currentForm = form.gunner;
        moveState = AnimState.transform;

        StartCoroutine(transformForm());
    }

    public void changeToParasite(Vector2 dir)
    {
        currentForm = form.parasite;
        moveState = AnimState.jump;

        rb.velocity = new Vector2(rb.velocity.x * 6.0f, 0);
        rb.velocity += Vector2.up * 20.0f;
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

    IEnumerator JumpWait(float time)
    {
        isLunging = true;

        yield return new WaitForSeconds(0.2f);

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * lungeHeight;

        rb.velocity += Vector2.right * playerDirection *lungeDash;
        if (moveInputX > 0.2f || moveInputX < -0.2f)
            rb.velocity = rb.velocity * new Vector2(0.9f, 1.0f);

        yield return new WaitForSeconds(time);
        isLunging = false;
    }

    IEnumerator DamageKnockback()
    {
        rb.velocity = new Vector2(-rb.velocity.x, 0);
        rb.velocity += Vector2.up * 6.0f;

        yield return new WaitForSeconds(0.6f);

        isTakingDamage = false;
    }

    IEnumerator transformForm()
    {
        transformIsFinished = false;

        yield return new WaitForSeconds(0.45f);

        transformIsFinished = true;
    }

    IEnumerator transformBack()
    {
        isTransformingToParasite = true;

        yield return new WaitForSeconds(0.75f);

        isTransformingToParasite = false;
    }
}

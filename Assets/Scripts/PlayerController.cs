using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;  // rigidbody
    private BoxCollider2D boxCollider2d; // boxcollider
    private Collision coll; // collision circles
    public enum form { parasite, gunner }
    public form currentForm;
    private enum AnimState { idle, walk, lunge };

    [Space]
    [Header("Base Game Stats")]
    public float walkSpeed = 4;
    public float lungeHeight = 4;
    public float lungeDash = 14;


    private float moveInputX; // Either -1 (left), 1 (right), or 0 (no input)
    private float moveInputY; // Similiar to X
    private float playerDirection = 1; // -1 or 1, similar to moveInputX

    public bool isGrounded;
    public bool isLunging = false;


    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void Walk(Vector2 dir)
    {
        rb.velocity = new Vector2(dir.x * walkSpeed, rb.velocity.y);
    }

    private void Lunge(Vector2 dir)
    {
        //rb.velocity = Vector2.zero;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * lungeHeight;
        rb.velocity += Vector2.right * playerDirection * lungeDash;

        StartCoroutine(LungeWait(0.7f));
    }

    // stops player controls for a second
    IEnumerator LungeWait(float time)
    {
        isLunging = true;

        yield return new WaitForSeconds(time);

        isLunging = false;
    }
}

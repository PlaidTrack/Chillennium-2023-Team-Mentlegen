using UnityEngine;

public class HumanController : MonoBehaviour
{
    private Collision enemyColl; // collision circles
    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;

    // Start is called before the first frame update
    void Start()
    {
        enemyColl = GetComponent<Collision>();
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 3.0f;
    }

    void FixedUpdate()
    {
        // changes direction based on whether the enemy is touching a wall
        if (enemyColl.onWall)
            dirX = (enemyColl.onLeftWall) ? 1 : -1;

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    void LateUpdate()
    {
        FaceCheck();
    }

    void FaceCheck()
    {
        // enemy is facing right if dirX is greater than 0
        facingRight = (dirX > 0) ? true : false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }
}

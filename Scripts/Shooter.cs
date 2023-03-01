using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Vector2 direction;
    private void FixedUpdate()
    {
        Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mouseDirection - transform.position;
        //direction = direction - new Vector2(14.0f, 14.0f);
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}

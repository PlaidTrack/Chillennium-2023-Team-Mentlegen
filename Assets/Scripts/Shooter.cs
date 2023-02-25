using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition); // gets mouse position
        Vector3 mouseForwardPoint = mousePos + (Camera.main.transform.forward * 10.0f);

        Vector3 rotation = mouseForwardPoint - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;   // fetches rotation on z axis by taking the arctangent of

        Debug.Log(rotZ);

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

    }
}

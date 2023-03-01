using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLaser : MonoBehaviour
{

    public GameObject beam;
    public Quaternion rotationModifier;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y, rot.z - 35);
            Instantiate(beam, transform.position, Quaternion.Euler(rot));
            player.audioSource.PlayOneShot(player.shoot);
        }
    }
}

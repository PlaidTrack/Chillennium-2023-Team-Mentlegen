using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(PolygonCollider2D))]

public class CameraTrigger2d : MonoBehaviour
{
    //[SerializeField] private CinemachineVirtualCamera cam;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("CAMERA!");
        if (other.gameObject.CompareTag("Player"))
        {
        }
    }
}

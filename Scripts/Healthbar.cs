using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalhealthBar;

    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth / 6.0f;
    }

    private void Update()
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth / 6.0f;
    }
}

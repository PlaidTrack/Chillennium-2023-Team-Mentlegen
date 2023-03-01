using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private PlayerController player;

    private void Awake()
    {
        currentHealth = startingHealth;
        player = GetComponent<PlayerController>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            player.isTakingDamage = true;
        } else
        {

        }
    }

    private void Update()
    {
        
    }
}

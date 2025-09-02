using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float playerHitPoints = 200f;
    public float PlayerHitPoints { get {return playerHitPoints; } }

    DeathHandler deathHandler;

    void Start()
    {
        deathHandler = FindObjectOfType<DeathHandler>();
    }

    public void TakeDamage(float damage)
    {
        playerHitPoints -= damage;
        if (playerHitPoints <= 0)
        {
            PlayerDeath();
        }
    }

    void PlayerDeath()
    {
        deathHandler.HandleDeath();
    }
}

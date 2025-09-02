 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    public float HitPoints { get { return hitPoints; } }

    public void DamageTarget(float damage)
    {
        BroadcastMessage("OnDamageTaken");
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            KillTarget();
        }
    }

    void KillTarget()
    {
        Destroy(gameObject);
    }
}

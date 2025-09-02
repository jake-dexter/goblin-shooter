using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] Camera fpCamera;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;

    [SerializeField] float gunDamage = 20f;
    [SerializeField] float gunRange = 100f;

    [SerializeField] AmmoType ammoType;
    [SerializeField] Ammo ammoSlot;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        ammoSlot.ReduceCurrentAmmo(ammoType);
        Recoil(animator);
    }

    public void Recoil(Animator animator)
    {
        animator.SetTrigger("shot");
    }

    void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    void ProcessRaycast()
    {
        if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out RaycastHit hit, gunRange))
        {
            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target == null) { return; }

            HitTarget(target);
        }
        else
        {
            return;
        }
    }

    void CreateHitImpact(RaycastHit hit)
    {
        GameObject VFX = Instantiate(hitEffect, hit.point,Quaternion.LookRotation(hit.normal));
        Destroy(VFX, 0.6f);
    }

    void HitTarget(EnemyHealth target)
    {
        target.DamageTarget(gunDamage);
    }

    void OnDisable()
    {
        if (animator.GetBool("scoped"))
        {
            animator.SetBool("scoped", false);
        }
    }
}

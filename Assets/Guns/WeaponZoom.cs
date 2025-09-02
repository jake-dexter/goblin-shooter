using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    [Header("Main Variables")]
    [SerializeField] Camera cam;
    [SerializeField] PlayerController controller;
    [SerializeField] int scopedFOV = 40;
    [SerializeField] float scopedSensitivity = 400f;

    [Header("Animator References")]
    [SerializeField] Animator pistolAnimator;
    [SerializeField] Animator shotgunAnimator;

    float zoomSpeed = 8f;
    
    bool isScoped;
    public bool IsScoped { get { return isScoped; } }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            isScoped = true;
            controller.ChangeFOV(scopedFOV, zoomSpeed);
            controller.ChangeSensitivity(scopedSensitivity);
        }
        else
        {
            isScoped = false;
            controller.ChangeSensitivity();
        }

        pistolAnimator.SetBool("scoped", isScoped);
        shotgunAnimator.SetBool("scoped", isScoped);
    }
}

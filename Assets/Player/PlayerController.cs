using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Controls")]
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;

    [Header("Mouse Controls")]
    [SerializeField] float sensitivity = 800f;

    [Header("View Controls")]
    [SerializeField] float fovScaleSpeed = 3f;
    [SerializeField] int walkFOV = 60;
    [SerializeField] int sprintFOV = 65;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    [Header("Pistol Weapon Animations")]
    [SerializeField] GameObject pistol;

    [Header("Shotgun Weapon Animations")]
    [SerializeField] GameObject shotgun;

    const float walkSpeed = 30f;
    const float sprintSpeed = 45f;
    const float defaultSensitivity = 800f;

    float yaw;
    float pitch;
    float currentSpeed;
    float groundDistance  = 0.4f;
    
    Rigidbody rb;

    WeaponZoom pistolZoom;
    WeaponZoom shotgunZoom;
    Animator pistolAnimator;
    Animator shotgunAnimator;

    bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();

        pistolZoom = pistol.GetComponent<WeaponZoom>();
        shotgunZoom = shotgun.GetComponent<WeaponZoom>();

        pistolAnimator = pistol.GetComponent<Animator>();
        shotgunAnimator = shotgun.GetComponent<Animator>();

        currentSpeed = walkSpeed;
        sensitivity = defaultSensitivity;
    }

    void Update()
    {
        PlayerLook();
        PlayerMovement();
        PlayerJump();
    }

    void PlayerLook()
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        yaw += Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;

        Camera.main.transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void PlayerMovement()
    {
        float x = Input.GetAxisRaw("Vertical");
        float z = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ChangeSpeed(sprintFOV, sprintSpeed);
        }
        else
        {
            ChangeSpeed(walkFOV, walkSpeed);
        }

        Vector2 axis = new Vector2(x,z).normalized * currentSpeed;
        Vector3 forward = new Vector3(-Camera.main.transform.right.z, 0f, Camera.main.transform.right.x);

        Vector3 xDirection = forward * axis.x;
        Vector3 yDirection = Vector3.up * rb.linearVelocity.y;
        Vector3 zDirection = Camera.main.transform.right * axis.y;

        Vector3 wishDirection = xDirection + yDirection + zDirection;
        
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) 
        {
            
            pistolAnimator.SetBool("moving", false);
            shotgunAnimator.SetBool("moving", false);
        }
        else 
        {
            pistolAnimator.SetBool("moving", true);
            shotgunAnimator.SetBool("moving", true);
        }

        rb.linearVelocity = wishDirection;
    }

    public void ChangeSensitivity(float passedSensitivity = defaultSensitivity)
    {
        sensitivity = passedSensitivity;
    }

    void ChangeSpeed(int fov, float speed = walkSpeed)
    {
        if (pistol.activeInHierarchy && !pistolZoom.IsScoped) { ChangeFOV(fov, fovScaleSpeed); }
        if (shotgun.activeInHierarchy && !shotgunZoom.IsScoped) { ChangeFOV(fov, fovScaleSpeed); }
        currentSpeed = speed;
    }

    public void ChangeFOV(int fov, float scaleSpeed)
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fov, scaleSpeed * Time.deltaTime);
    }

    void PlayerJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Vector3 jump = new Vector3(0f, Mathf.Sqrt(jumpHeight * -2f * gravity), 0f);
            rb.linearVelocity = jump;
        }
    }
}

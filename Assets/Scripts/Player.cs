using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Linq;

public class Player : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Animator[] animators;

    private CinemachineVirtualCamera cam;
    // [SerializeField] private Animator animator_general;
    [SerializeField] private ParticleSystem footdust_moving;
    [SerializeField] private ParticleSystem.EmissionModule footdustemission_moving;
    [SerializeField] private ParticleSystem footdust_landing;
    private Rigidbody2D rb;
    private GroundCheck groundCheck;
    public InputMaster controls;

    private bool hasReleasedJump;
    private float lastHorizontalInput;
    private float currentAccelerationTime;
    private int lastDir;
    private bool jumped;
    private float originalSpeed;
    private bool vulnerable = true;
    private float originalEmission;
    private float originalEmission_ghost;

    [Header("Basic")]

    [SerializeField] private float speed = 5.5f;
    [SerializeField] private float jumpHeight = 7f;

    [Header("Advanced")]
    [SerializeField] private float accelerationTime = .01f;
    [SerializeField] private float decelerationTime = .08f;
    [Space]

    [SerializeField] private float releaseFactor = .25f;
    [SerializeField] private float downGravityMultiplier = 1f;
    [SerializeField] private float normalGravityMultiplier = 1f;
    [SerializeField] private float maxYVelocity = 15f;

    [Header("Assist")]

    [SerializeField] private float coyoteTime = .037f;
    private float currentCoyoteTime;

    [SerializeField] private float jumpBufferTime = .05f;
    private float currentJumpBufferTime;

    public void ChangeCharacterState(bool idle)
    {
        foreach (var item in animators)
        {
            item.SetBool("Idle", idle);
        }

    }

    public void Die()
    {
        PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths") + 1);
        FindObjectOfType<Crossfade>().LoadWrapper(SceneManager.GetActiveScene().buildIndex);
        Destroy(gameObject);
    }

    private void Awake()
    {
        //animator_alive = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cam = FindAnyObjectByType<CinemachineVirtualCamera>();
        groundCheck = transform.GetComponentInChildren<GroundCheck>();

        controls = new InputMaster();
        controls.Player.Jump.performed += _ => ResetJumpBuffer();
        controls.Player.Jump.canceled += _ => ReleaseJump();

        originalSpeed = speed;
        groundCheck.onHitGround += ResetHasReleasedJump;

        footdustemission_moving = footdust_moving.emission;
        originalEmission = footdustemission_moving.rateOverTimeMultiplier;


    }
    private void ResetHasReleasedJump()
    {
        if (jumped)
        {
            //animator_general.SetTrigger("Squash");
            //AudioManager.instance.Play("Landing");

            footdust_landing.gameObject.SetActive(true);
            footdust_landing.Stop();
            footdust_landing.transform.position = footdust_moving.transform.position;
            footdust_landing.Play();
        }
        hasReleasedJump = false;
        jumped = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
        groundCheck.onHitGround -= ResetHasReleasedJump;
    }
    private void LateUpdate()
    {
        speed = originalSpeed;
    }

    private void ReleaseJump()
    {
        if (!hasReleasedJump && rb.velocity.y > 1 && jumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * releaseFactor);
            hasReleasedJump = true;
            Debug.Log("Jumped Released");
        }
    }
    private void Jump()
    {
        //animator_general.SetTrigger("Stretch");
        // AudioManager.instance.Play("Jump");

        rb.velocity = (new Vector2(rb.velocity.x, jumpHeight));
        jumped = true;

        Debug.Log("Jumped");
    }
    private void ResetJumpBuffer() => currentJumpBufferTime = jumpBufferTime;
    void Update()
    {
        if (transform.position.y  < -18)
        {
            Die();
        }
        if (currentJumpBufferTime > 0 && currentCoyoteTime > 0)
        {
            Jump();
            currentJumpBufferTime = 0f;
        }
        else
        {
            currentJumpBufferTime -= Time.deltaTime;
        }

        if (groundCheck.IsGrounded)
        {
            currentCoyoteTime = coyoteTime;
        }
        else
        {
            currentCoyoteTime -= Time.deltaTime;
        }


        float horizontalInput = controls.Player.Move.ReadValue<float>();


        if (Math.Abs(horizontalInput) > 0)
        {
            ChangeCharacterState(false);
        }
        else
        {
            ChangeCharacterState(true);
        }

        if (horizontalInput == 0)
        {
            currentAccelerationTime -= Time.deltaTime * (1 / decelerationTime * accelerationTime);
            if (currentAccelerationTime < 0) currentAccelerationTime = 0;
        }
        else if (horizontalInput != lastHorizontalInput)
        {
            currentAccelerationTime = 0f;
        }
        else
        {
            lastDir = MathF.Sign(horizontalInput);

            currentAccelerationTime += Time.deltaTime;

            if (currentAccelerationTime > accelerationTime) currentAccelerationTime = accelerationTime;
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = downGravityMultiplier;
        }
        else
        {
            rb.gravityScale = normalGravityMultiplier;
        }

        // sets rotation accordingly to velocity
        if (Mathf.Abs(horizontalInput) > 0)
        {
            transform.eulerAngles = new Vector3(0, (horizontalInput > 0) ? 0 : 180, 0);
        }

        //animator_alive.SetFloat("x Input", Mathf.Abs(horizontalInput));
        //animator_alive.SetBool("isGrounded", groundCheck.IsGrounded);

        rb.velocity = new Vector2(speed * lastDir * (currentAccelerationTime / accelerationTime), Math.Clamp(rb.velocity.y, -maxYVelocity, Mathf.Infinity));

        lastHorizontalInput = horizontalInput;


        // Foot Dust

        if (horizontalInput != 0 && groundCheck.IsGrounded)
        {
            footdustemission_moving.rateOverTime = originalEmission;
        }
        else
        {
            footdustemission_moving.rateOverTime = 0;
        }
    }
}
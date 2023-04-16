using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRobot : Robot
{
    private Animator anim;
    private Rigidbody2D rb;
    private InputMaster controls;

    [SerializeField] private float speed;
    [Header("Advanced")]
    [SerializeField] private float accelerationTime = .01f;
    [SerializeField] private float decelerationTime = .08f;
    private float lastHorizontalInput;
    private float currentAccelerationTime;
    private int lastDir;
    private void Awake()
    {
        anim = transform.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        controls = FindObjectOfType<Player>().controls;
    }
    public override void Activate()
    {
        anim.SetBool("Active", true);
        active = true;
    }
    public override void Deactivate()
    {
        anim.SetBool("Active", false);
        active = false;
    }

    private void Update()
    {
        if (active)
        {
            float horizontalInput = controls.Robot.Horizontal.ReadValue<float>();
            anim.SetFloat("x Input", MathF.Abs(horizontalInput));

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



            rb.velocity = new Vector2(speed * lastDir * (currentAccelerationTime / accelerationTime), rb.velocity.y);
            lastHorizontalInput = horizontalInput;
        }
        else
        {
            rb.velocity = new(0, rb.velocity.y);
            currentAccelerationTime = 0;
        }
    }
}

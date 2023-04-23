using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SpikeyRobot : Robot
{
    private Animator anim;
    private Rigidbody2D rb;

    private List<Enemy> enemies = new();
    [SerializeField] private float speed;
    [Header("Advanced")]
    [SerializeField] private float accelerationTime = .01f;
    [SerializeField] private float decelerationTime = .08f;
    private float lastHorizontalInput;
    private float currentAccelerationTime;
    private int lastDir;

    protected override void Start()
    {
        base.Start();
    }
    private void Awake()
    {
        anim = transform.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public override void Activate()
    {
        base.Activate();
        anim.SetBool("Active", true);
    }
    public override void Deactivate()
    {
        base.Deactivate();
        anim.SetBool("Active", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Crate>() != null)
        {
            collision.transform.SetParent(transform);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (!active)
        {
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
            return;
        }
        Player player = collision.gameObject.GetComponent<Player>();

        if (enemy != null)
        {
            enemy.Die();
        }

        if (player != null)
        {
            player.Die();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null) enemies.Remove(enemy);
        if (collision.gameObject.GetComponent<Crate>() != null)
        {
            collision.transform.SetParent(null);
        }
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
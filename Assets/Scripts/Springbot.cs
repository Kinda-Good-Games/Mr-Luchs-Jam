using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springbot : Robot
{
    [SerializeField] private float jumpHeight;
    private Rigidbody2D rb;
    private GroundCheck groundCheck;
    private Animator anim;
    private bool jumped;
    private bool hasReleasedJump;
    [SerializeField] private float releaseFactor;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        controls.Robot.Vertical.performed += _ => Jump(_.ReadValue<float>());
        controls.Robot.Vertical.canceled += _ => ReleaseJump();
        groundCheck.onHitGround += ResetHasReleasedJump;
    }
    private void OnDisable()
    {
        controls.Robot.Vertical.performed -= _ => Jump(_.ReadValue<float>());
        controls.Robot.Vertical.canceled -= _ => ReleaseJump();
        groundCheck.onHitGround -= ResetHasReleasedJump;
    }
    private void ReleaseJump()
    {
        Debug.Log("uwu");
        if (!hasReleasedJump && rb.velocity.y > 0 && jumped)
        {
            Debug.Log("asdg");
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * releaseFactor);
            hasReleasedJump = true;
        }
        jumped = true;
    }
    private void ResetHasReleasedJump()
    {
        if (jumped)
        {
            //animator_general.SetTrigger("Squash");
            //AudioManager.instance.Play("Landing");

            /*footdust_landing.gameObject.SetActive(true);
            footdust_landing.Stop();
            footdust_landing.transform.position = footdust_moving.transform.position;
            footdust_landing.Play();*/
        }
        hasReleasedJump = false;
        jumped = false;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.GetComponentInChildren<GroundCheck>();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        anim.SetBool("Active", false);
    }
    public override void Activate()
    {
        base.Activate();
        anim.SetBool("Active", true);
    }

    private void Jump(float input)
    {
        if (!groundCheck.IsGrounded) return;
        if (!active) return;

        anim.SetTrigger("Jump");
        rb.AddForce(new(0, jumpHeight), ForceMode2D.Impulse);
        jumped = true;
    }
}

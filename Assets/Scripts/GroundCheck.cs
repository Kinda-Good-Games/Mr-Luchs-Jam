using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded { private set; get; }
    [SerializeField] private string reqTag = "Ground";

    public delegate void OnHitGround();
    public event OnHitGround onHitGround;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("QEAf");
        if (collision.CompareTag(reqTag))
        {
            IsGrounded = true;
            onHitGround();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(reqTag))
        {
            IsGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(reqTag))
        {
            IsGrounded = false;
        }
    }
}

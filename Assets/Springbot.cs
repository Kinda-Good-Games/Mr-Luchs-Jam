using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springbot : Robot
{
    [SerializeField] private float jumpHeight;
   private Rigidbody2D rb;
    protected override void Start()
    {
        base.Start();
        controls.Robot.Vertical.performed += _ => Jump(_.ReadValue<float>());
    }
    private void OnDisable()
    {
        controls.Robot.Vertical.performed -= _ => Jump(_.ReadValue<float>());
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Jump(float input)
    {
        if (input <= 0) return;
        rb.AddForce(new(0, jumpHeight), ForceMode2D.Impulse);
    }
}

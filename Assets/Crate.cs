using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private GameManager gm;
    private Animator anim;
    private Collider2D col;
    private Rigidbody2D rb;

    [SerializeField] private string color;
    private bool isActive;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        gm.onColorChanged += HandleColorChange;
    }
    private void OnDisable()
    {
        gm.onColorChanged -= HandleColorChange;
    }

    // Update is called once per frame
    void Update()
    {
        col.isTrigger = isActive;
        //rb.gravityScale = isActive ? 0: 1;
    }

    private void HandleColorChange(string key, bool value)
    {
        if (key != color) return;

        isActive = value;
        anim.SetBool("Active", value);
    }
}

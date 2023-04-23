using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    [SerializeField] private string color;
    private Animator anim;

    private GameManager gameManager;
    private List<Transform> currentInMe = new();
    private bool isActive;
    private bool hasSetFalse;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

    }
    private void Update()
    {
        if (!isActive)
        {
            if (hasSetFalse) return;

            hasSetFalse = true;
        }
        gameManager.SetData(color, isActive);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponent<Robot>() != null || collision.GetComponent<Crate>() != null)
        {
            currentInMe.Add(collision.transform);
        }

        isActive = currentInMe.Count > 0;
        hasSetFalse = false;

        anim.SetBool("Active", currentInMe.Count > 0);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponent<Robot>() != null || collision.GetComponent<Crate>() != null)
        {
            currentInMe.Remove(collision.transform);
        }
        isActive = currentInMe.Count > 0;
        hasSetFalse = false;


        anim.SetBool("Active", currentInMe.Count > 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    [SerializeField] private string color;
    [SerializeField] private Animator anim;

    private GameManager gameManager;
    private List<Transform> currentInMe = new();

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponent<Robot>() != null)
        {
            currentInMe.Add(collision.transform);
        }
        gameManager.SetData(color, currentInMe.Count > 0);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponent<Robot>() != null)
        {
            currentInMe.Remove(collision.transform);
        }
        gameManager.SetData(color, currentInMe.Count > 0);
    }
}

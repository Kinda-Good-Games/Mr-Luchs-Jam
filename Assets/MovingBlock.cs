using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] private string color;
    [SerializeField] private float speed;
    [SerializeField] private float checkDistance;

    [SerializeField] private float lerpTime;
    private float lerpCounter;

    private bool isActive;

    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private Vector2 raycastDir;
    private Vector2 blockPosition;
    private Vector2 originalPosition;

    private Rigidbody2D rb;

    private GameManager gm;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();

        originalPosition = transform.position;
        blockPosition= transform.position;
    }
    private void OnEnable()
    {
        gm.onColorChanged += HandleColorChange;
    }
    private void OnDisable()
    {
        gm.onColorChanged -= HandleColorChange;
    }
    private void HandleColorChange(string key, bool value)
    {
        if (key != color) return;

        if (isActive && value != isActive)
        {
            blockPosition = transform.position;
            Debug.Log("q");
            lerpCounter = 0;
        }
        isActive = value;
    }
    public void Update()
    {
        if (isActive)
        {
            RaycastHit2D result;//
            if (!Physics2D.Raycast(transform.position, raycastDir, checkDistance, groundLayer))
            {
                rb.velocity = raycastDir * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            lerpCounter += Time.deltaTime;

            transform.position = Vector2.Lerp(blockPosition, originalPosition, lerpCounter/lerpTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<Robot>() != null)
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<Robot>() != null)
            collision.transform.SetParent(null);
    }
}

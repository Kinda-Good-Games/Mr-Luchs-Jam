using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private string color;
    [SerializeField] private float speed;
    [SerializeField] private float checkDistance;

    [SerializeField] private float lerpTime;
    private float lerpCounter;

    private bool isActive;

    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private Vector2 raycastDir;
    [SerializeField]private Vector2 offset;
    private Vector2 blockPosition;
    private Vector2 originalPosition;

    private Rigidbody2D rb;

    private GameManager gm;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();

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
        anim.SetBool("Active", value);
    }
    public void Update()
    {
        if (isActive)
        {
            var hits = Physics2D.RaycastAll((Vector2)transform.position + offset, raycastDir, checkDistance, groundLayer);//

            List<RaycastHit2D> trueHits = hits.ToList().FindAll(x => x.transform.gameObject!= gameObject);
            if (trueHits.Count <= 0)
            {
                transform.Translate((raycastDir * speed * Time.deltaTime));
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
        {
           // collision.transform.SetParent(transform);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<Robot>() != null)
        {
           // collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
           // collision.transform.SetParent(null);
        }
    }
}

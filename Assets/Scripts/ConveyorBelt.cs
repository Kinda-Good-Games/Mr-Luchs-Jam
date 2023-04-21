using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private string color;
    [SerializeField] private float extraSpeedTime;
    [SerializeField] private Vector2 extraVelocity;

    private List<GameObject> currentInMe = new();
    private bool isActive;
    private GameManager gm;
    private Animator anim;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<Robot>() != null)
        {
            currentInMe.Add(collision.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("51");
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<Robot>() != null)
        {
            StartCoroutine(RemoveObject(collision.gameObject));
        }
    }
    private IEnumerator RemoveObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(extraSpeedTime);
        currentInMe.Remove(gameObject);

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

        isActive = value;
        anim.SetBool("Active", value);
    }

    private void LateUpdate()
    {
        if (isActive) return;
        foreach (var item in currentInMe)
        {
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();

            if (rb == null) continue;
           if (rb.velocity == extraVelocity) continue;

            rb.velocity += extraVelocity;
        }
    }
}

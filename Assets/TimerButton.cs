using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerButton : MonoBehaviour
{
    [SerializeField] private string color;
    [SerializeField] private float activeTime;
    private float activeCounter;

    private bool LastValue
    {
        get { return lastValue; }
        set
        {
            if (value)
            {
                lastValue = value;
            }
            else
            {
                if (currentInMe.Count > 0)
                {
                    activeCounter = activeTime;
                }
                lastValue = activeCounter > 0;
                anim.SetBool("Active", activeCounter > 0);
            }
        }
    }
    private bool lastValue = false;
    private GameManager gameManager;
    private List<Transform> currentInMe = new();
    private Animator anim;
    private Transform bar;
    private float originalBarSize;
    private float originalBarHeight;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        bar = transform.GetChild(0);
        originalBarSize = bar.localScale.x;
        originalBarHeight = bar.localScale.y;
    }
    private void SetData()
    {
        Debug.Log("2");
        activeCounter = activeTime;
        // if the color isn't  active, then reactivate it
        if (!gameManager.datas.Find(x => x.color == color).value) gameManager.SetData(color, true);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponent<Robot>() != null)
        {
            currentInMe.Add(collision.transform);
        }

        if (currentInMe.Count > 0)
        {
            Debug.Log("5");
            SetData();
        }

        anim.SetBool("Active", activeCounter > 0);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("51");
        if (collision.CompareTag("Player") || collision.GetComponent<Robot>() != null)
        {
            currentInMe.Remove(collision.transform);
        }
        if (currentInMe.Count > 0)
        {
            SetData();
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (currentInMe.Count > 0)
        {
            activeCounter = activeTime;
        }
        activeCounter -= Time.deltaTime;
        if (LastValue != activeCounter > 0)
        {
            LastValue = activeCounter > 0;
            gameManager.SetData(color, LastValue);
            Debug.Log("1" + (activeCounter > 0) + " && " + LastValue);
        }

        bar.localScale = new Vector2(Mathf.Clamp(activeCounter / activeTime * originalBarSize, 0, 1), originalBarHeight);

        anim.SetBool("Active", activeCounter > 0);
    }
}

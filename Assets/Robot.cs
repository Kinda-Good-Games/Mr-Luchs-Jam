using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public bool active = false;
    private Vector2 originalPosition;

    protected InputMaster controls;
    protected virtual void Start()
    {
        controls = FindObjectOfType<Player>().controls;
        originalPosition = transform.position;
    }
    public virtual void Activate()
    {
        active = true;
    }
    public virtual void Deactivate()
    {
        active = false;
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
    }
}

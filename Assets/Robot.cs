using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public bool active = false;

    public virtual void Activate()
    {
        Debug.Log("Active!");
    }
    public virtual void Deactivate()
    {
        Debug.Log("Inactive!");
    }
}

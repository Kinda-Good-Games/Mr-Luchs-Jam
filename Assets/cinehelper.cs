using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinehelper : MonoBehaviour { 
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -Mathf.Abs(transform.position.z));
    }
}

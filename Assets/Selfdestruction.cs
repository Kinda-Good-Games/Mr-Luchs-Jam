using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestruction : MonoBehaviour
{
    [SerializeField] private float deathTime;

    private void Awake()
    {
        Destroy(gameObject, deathTime);
    }
}

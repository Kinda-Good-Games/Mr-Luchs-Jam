using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private float magnitude;
    [SerializeField] private float duration;
    [SerializeField] private float frequency;

    public void Shake()
    {
        FindObjectOfType<CameraShake>().ShakeCamera(magnitude, duration, frequency, true);
    }
}

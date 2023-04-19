using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TopPipe : MonoBehaviour
{
    [SerializeField] private string color;
    private GameManager gm;
    private bool isActive;
    [SerializeField] private LayerMask occupationLayer;
    [SerializeField] private Vector2 raycastDir;
    [SerializeField] private float checkDistance;
    [SerializeField] private float spawnTime;
    private float spawnCounter;
    [SerializeField] private Transform spawnpoint;
    [SerializeField] private GameObject spawnable;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
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
    }
    private void OnDrawGizmos()
    {
    }

    private void Update()
    {
        RaycastHit2D result;//
        if (!Physics2D.Raycast(spawnpoint.position, raycastDir, checkDistance, occupationLayer))
        {
            Debug.DrawLine(spawnpoint.position, (Vector2)spawnpoint.position + (raycastDir * checkDistance));
            spawnCounter -= Time.deltaTime;
        }
        else
        {
            spawnCounter = spawnTime * 0.12f;
        }

        if (spawnCounter < 0)
        {
            spawnCounter = spawnTime;
            Instantiate(spawnable, spawnpoint.position, Quaternion.identity);
        }
    }
}
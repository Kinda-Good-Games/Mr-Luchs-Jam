using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Camera Shake")]
    [SerializeField] private float magnitude;
    [SerializeField] private float duration;


    [Space]
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask occupationLayer;
    [SerializeField] private Vector2 raycastDir;
    [SerializeField] private Vector2 offset = new Vector2(0.1f, 0);
    [SerializeField] private float checkDistance;
    [SerializeField] private float checkCooldown;
    private float checkCooldownTimer;
    private bool canCheck;

    private float dir = 0;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb.velocity = new Vector2(speed, 0) * transform.right + new Vector2(0, rb.velocity.y);
        checkCooldownTimer -= Time.deltaTime;
        canCheck = checkCooldownTimer < 0;

        if (!canCheck) return;

        var hits = Physics2D.RaycastAll((Vector2)transform.position + offset, raycastDir * transform.right, checkDistance, occupationLayer);
        Debug.DrawLine(rb.position + offset, rb.position + offset + (raycastDir * transform.right * checkDistance), Color.red);
        List<RaycastHit2D> trueHits = hits.ToList().FindAll(x => x.transform != transform && !x.collider.isTrigger && x.collider.gameObject.GetComponent<PlatformEffector2D>() == null);
        if (trueHits.Count > 0)
        {
            canCheck = false;
            checkCooldownTimer = checkCooldown;


            dir = dir == 0 ? -1 : 0;
            transform.rotation = Quaternion.Euler(0, 180 * dir, 0);
        }
    }
    public void Die()
    {
        CameraShake.instance.ShakeCamera(magnitude, duration, smoothTransition:true);
        Destroy(gameObject);
        var obj= Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(obj, obj.main.duration + 0.1f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        Robot robot = collision.gameObject.GetComponent<Robot>();
        if (player)
        {
            player.Die();
        }
        if (robot)
        {
            if (robot.GetType() == typeof(SpikeyRobot)) return;
            robot.ResetPosition();
            robot.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        dir = dir == 0 ? -1 : 0;
        transform.rotation = Quaternion.Euler(0, 180 * dir, 0);
    }
}

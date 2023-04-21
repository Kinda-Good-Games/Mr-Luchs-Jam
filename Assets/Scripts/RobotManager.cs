using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    private List<Robot> robots = new List<Robot>();
    private int current;
    private bool isActive;
    [SerializeField] private Transform crosshair;
    private Animator crosshair_animator;
    private InputMaster controls;
    [SerializeField] private float lerpTime;
    private bool isLerping = false;
    private void Start()
    {
        controls = FindObjectOfType<Player>().controls;
        robots = FindObjectsOfType<Robot>().ToList();
        crosshair_animator = crosshair.GetComponent<Animator>();


        controls.Robot.Interact.performed += ctx => Interact();
        controls.Robot.Horizontal.performed += ctx => ChangeCurrent(ctx.ReadValue<float>());

        StartCoroutine(LerpToRobot(robots[current].transform, lerpTime));
    }
    private void OnDisable()
    {

        controls.Robot.Horizontal.performed -= ctx => ChangeCurrent(ctx.ReadValue<float>());
        controls.Robot.Interact.performed -= ctx => Interact();
    }
    private void Interact()
    {
        if (!isActive)
        {
            crosshair_animator.SetTrigger("Activate");
            robots[current].Activate();
            crosshair.position = robots[current].transform.position;
        }
        else
        {
            StartCoroutine(LerpToRobot(robots[current].transform, 0));
            crosshair.position = robots[current].transform.position;
            crosshair_animator.SetTrigger("Deactivate");
            robots[current].Deactivate();
        }
        isActive = !isActive;
    }
    private void ChangeCurrent(float value)
    {
        if (robots.Count == 0) return;
        if (isActive) return;
        StopAllCoroutines();
        if (Mathf.Sign(value) == 1)
        {
            if (current < robots.Count - 1)
            {
                current += (int)value;
            }
            else
            {
                current = 0;
            }
        }
        else
        {
            if (current <= 0)
            {

                current = robots.Count - 1;
            }
            else
            {
                current += (int)value;
            }
        }

        StartCoroutine(LerpToRobot(robots[current].transform, lerpTime));
    }
    private IEnumerator LerpToRobot(Transform tr, float timeToLerp)
    {
        float elapsed = 0;
        isLerping = true;
        while (elapsed < timeToLerp)
        {
            float progress = elapsed / timeToLerp;

            crosshair.position = Vector2.Lerp(crosshair.position, tr.position, progress);


            elapsed += Time.deltaTime;
            yield return null;
        }
        isLerping = false;
    }
    private void Update()
    {
        if (robots.Count <= 1) return;    

        if (!isActive && !isLerping)
        {
            crosshair.position = robots[current].transform.position;
        }
        if (robots.Count == 0)
        {
            crosshair.position = new Vector3(124346, 124132);
        }
    }


}

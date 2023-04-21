using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    [SerializeField] private float loadDelay;
    private Crossfade crossfade;
    private Animator camAnim;

    private void Awake()
    {
        crossfade = FindObjectOfType<Crossfade>();
        camAnim = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().GetComponent<Animator>();
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        crossfade.LoadWrapper(sceneIndex);

        yield return new WaitForSeconds(loadDelay);
        camAnim.SetTrigger("Zoom");
    }
}

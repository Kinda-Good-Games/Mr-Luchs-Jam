using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crossfade : MonoBehaviour
{
    [SerializeField] private float sceneLoadingDelay;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void AllowZoom()
    {
        FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().GetComponent<Animator>().SetTrigger("Start");
    }
    public void LoadWrapper(int index)
    {
        StartCoroutine(LoadScene(index));
    }
    public IEnumerator LoadScene(int buildIndex)
    {
        animator.SetTrigger("Start");
        //AudioManager.instance.Play("Transition");
        //FindObjectOfType<AudioManager>().Play("Transition");

        yield return new WaitForSecondsRealtime(sceneLoadingDelay);

        Debug.Log("Load");
        SceneManager.LoadScene(buildIndex);
    }
    public IEnumerator LoadScene(string sceneName)
    {
        animator.SetTrigger("Start");
        Debug.Log("Loady");
        //AudioManager.instance.Play("Transition");
        //FindObjectOfType<AudioManager>().Play("Transition");
        yield return new WaitForSecondsRealtime(sceneLoadingDelay);

        Debug.Log("Load");
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
}

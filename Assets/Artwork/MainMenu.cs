using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void LoadGame()
    {
        AudioManager.instance.StopAll();
        StartCoroutine(FindObjectOfType<Crossfade>().LoadScene("Section" + PlayerPrefs.GetInt("Progress", 0)));
        AudioManager.instance.Play("Start");
        AudioManager.instance.Play("Welcome in Hell");
    }
    public void SetCreditsMenu(bool active)
    {
        //if (active) AudioManager.instance.Play("Button Click");

        if (active)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            animator.SetTrigger("Close");
        }
    }
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(FindObjectOfType<Crossfade>().LoadScene(0));
    }
}

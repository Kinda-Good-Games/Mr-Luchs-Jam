using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Untertitel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI subtitles;
    [SerializeField] private float timeBeforeDissapearence;
    [SerializeField] private string audioName;
    [SerializeField] private string subtitle;

    private void Awake()
    {
        subtitles.text = subtitle;
        AudioManager.instance.Play(audioName);
    }
}

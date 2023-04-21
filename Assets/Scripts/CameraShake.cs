using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private CinemachineBasicMultiChannelPerlin cam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        if (cam == null) return;

        cam.m_FrequencyGain = 0;
        cam.m_AmplitudeGain = 0;
    }
    public void ShakeCamera(float magnitude, float duration, float frequency = 1, bool smoothTransition = false)
    {
        //if (!SettingsMenu.settings.enabledCameraShake) return;

        if (cam == null)
        {
            cam = FindObjectOfType<CinemachineBasicMultiChannelPerlin>();
        }

        StartCoroutine(Shake(magnitude, duration, frequency, smoothTransition));
    }

    private IEnumerator Shake(float magnitude, float duration, float frequency, bool smoothTransition)
    {
        float time = duration;
        cam.m_FrequencyGain = frequency;
        cam.m_AmplitudeGain = magnitude;

        while (time > 0)
        {
            time -= Time.deltaTime;

            if (smoothTransition)
            {
                cam.m_AmplitudeGain = magnitude * (time / duration);
            }
            Debug.Log("Shakey Shake Shake");

            yield return null;
        }

        cam.m_FrequencyGain = 0;
        cam.m_AmplitudeGain = 0;

    }
}

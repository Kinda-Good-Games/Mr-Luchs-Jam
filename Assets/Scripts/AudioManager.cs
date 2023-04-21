using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public enum WhichAudioMixer
{
    Music,
    Soundeffect,
    Voiceline
}

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioMixerGroup MusicMixer;
    public AudioMixerGroup SoundeffectMixer;
    public AudioMixerGroup VoiceLineMixer;
    public static AudioManager instance;
    public List<Sound> sounds;
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
            return;
        }


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
            s.source.loop = s.Loop;
            s.source.playOnAwake = s.PlayOnAwake;
            if (s.output == WhichAudioMixer.Music)
            {
                s.source.outputAudioMixerGroup = MusicMixer;
            }
            else if (s.output == WhichAudioMixer.Soundeffect)
            {
                s.source.outputAudioMixerGroup = SoundeffectMixer;
            }
            else if (s.output == WhichAudioMixer.Voiceline)
            {
                s.source.outputAudioMixerGroup = VoiceLineMixer;
            }
            if (s.PlayOnAwake)
            {
                s.source.Play();
            }

        }
    }
    public void Play(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError(name + " doesnt exist");
            return;
        }
        s.source.Play();
        Debug.Log("Played: " + s.name);
    }
    public void Play(string name, float additionalTime)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError(name + " doesnt exist");
            return;
        }
        s.source.Play();
        StartCoroutine(s.CheckForEnd(additionalTime));
        Debug.Log("Played: " + s.name);
    }
    public void Stop(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null && s.source.isPlaying)
        {
            Debug.LogError(name + " doesnt exist and couldnt be stopped");
            return;
        }
        s.source.Stop();
    }
    public void StopAll()
    {
        foreach (var s in sounds)
        {
            Stop(s.name);
        }
    }
}
public delegate void OnSoundEnded();
[System.Serializable]
public class Sound
{
    public string name;
    public WhichAudioMixer output;
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume = 1f;
    [Range(.1f, 3f)]
    public float Pitch = 1f;
    [HideInInspector]
    public AudioSource source;
    public bool PlayOnAwake;
    public bool Loop;

    public OnSoundEnded onSoundEnded;

    public IEnumerator CheckForEnd(float additionalTime)
    {
        while (true)
        {
            if (!source.isPlaying)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(additionalTime);
        onSoundEnded?.Invoke();
        Debug.Log("end");
    }
}

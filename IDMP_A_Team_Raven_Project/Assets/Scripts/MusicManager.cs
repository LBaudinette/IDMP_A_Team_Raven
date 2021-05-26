using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{

    public AudioClip ambient;
    public AudioClip battle;
    public float fadeTime;
    public float volume;

    private bool activateFunc;
    private AudioSource musicSource;
    

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.clip = ambient;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fadeToBattle()
    {
        StartCoroutine(fadeTo(battle));
    }

    public void fadeToAmbient()
    {
        StartCoroutine(fadeTo(ambient));
    }

    private IEnumerator fadeTo(AudioClip targetAudio)
    {
        float currentTime = 0;
        float startVolume = musicSource.volume;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeTime);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = targetAudio;
        musicSource.Play();
        currentTime = 0;
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, volume, currentTime / fadeTime);
            yield return null;
        }
    }
}

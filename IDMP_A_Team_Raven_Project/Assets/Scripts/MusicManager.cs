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

    public bool activateFunc;
    private AudioSource musicSource;
    private bool inBattle;
    

    // Start is called before the first frame update
    void Start()
    {
        inBattle = false;
        activateFunc = false;
        musicSource = GetComponent<AudioSource>();
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.clip = ambient;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (activateFunc)
        {
            fadeToBattle();
            activateFunc = false;
        }
    }

    public void fadeToBattle()
    {
        inBattle = true;
        StartCoroutine(fadeTo(battle));
    }

    public void fadeToAmbient()
    {
        inBattle = false;
        StartCoroutine(fadeTo(ambient));
    }

    private IEnumerator fadeTo(AudioClip targetAudio)
    {
        float currentTime = 0;
        float startVolume = musicSource.volume;
        float endVolume = 0f;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, endVolume, currentTime / fadeTime);
            yield return null;
        }

        //musicSource.Stop();
        musicSource.clip = targetAudio;
        musicSource.Play();
        
        currentTime = 0f;
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(endVolume, volume, currentTime / fadeTime);
            yield return null;
        }
    }

    public bool isInBattle()
    {
        return this.inBattle;
    }
}

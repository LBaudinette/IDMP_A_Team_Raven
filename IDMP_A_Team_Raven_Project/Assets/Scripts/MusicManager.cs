using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{

    public AudioClip ambient;
    public AudioClip battle;

    private AudioSource musicSource;
    public float volume;

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
}

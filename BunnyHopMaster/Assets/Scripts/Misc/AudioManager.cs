using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClipContainer audioClips;
    private AudioSource audiosource;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        audiosource.loop = false;
    }

    void Update()
    {
        if (!audiosource.isPlaying)
        {
            audiosource.clip = GetRandomClip();
            audiosource.Play();
        }
    }

    private AudioClip GetRandomClip()
    {
        return audioClips.audioClips[Random.Range(0, audioClips.audioClips.Length)];
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}

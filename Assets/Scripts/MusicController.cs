using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static MusicController instance;

    private AudioSource audioSrc;
    private float musicVolume = 1f;
    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSrc.volume = musicVolume;
    }

    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}

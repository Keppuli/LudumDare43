using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance = null; // Singleton

    AudioSource aSource;
    public AudioClip sacrifice;

    void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        aSource = GetComponent<AudioSource>();
	}

    public void Play()
    {
        aSource.PlayOneShot(sacrifice);
    }
}

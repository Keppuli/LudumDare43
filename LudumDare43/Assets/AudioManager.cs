using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance = null; // Singleton

    AudioSource aSource;
    public AudioClip sacrifice;
    public AudioClip lighting;
    public AudioClip soul;
    public AudioClip growl;
    public AudioClip priestDie;
    public AudioClip priestEscape;

    void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
    }

    void Start () {
        aSource = GetComponent<AudioSource>();
	}
    public void Stop()
    {
        aSource.clip = null;
    }
    public void PlaySacrifice()
    {
        aSource.PlayOneShot(sacrifice);
    }
    public void PlayLighting()
    {
        aSource.PlayOneShot(lighting);
    }
    public void PlayGrowl()
    {
        aSource.PlayOneShot(growl);
    }
    public void PlaySoul()
    {
        aSource.PlayOneShot(soul);
    }
    public void PlayPriestDie()
    {
        aSource.PlayOneShot(priestDie);
    }
    public void PlayPriestEscape()
    {
        aSource.PlayOneShot(priestEscape);
    }
}

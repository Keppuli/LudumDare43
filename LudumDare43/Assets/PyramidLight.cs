using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidLight : MonoBehaviour {
    public static PyramidLight instance = null; // Singleton

    SpriteRenderer sr;
    public Sprite[] lights;
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
		sr = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
        UpdateSprite();

    }
    public void UpdateSprite()
    {
        if (GameManager.instance.sunPower > 90)
            sr.sprite = lights[8];
        else if (GameManager.instance.sunPower > 80)
            sr.sprite = lights[7];
        else if (GameManager.instance.sunPower > 70)
            sr.sprite = lights[6];
        else if (GameManager.instance.sunPower > 60)
            sr.sprite = lights[5];
        else if (GameManager.instance.sunPower > 50)
            sr.sprite = lights[4];
        else if (GameManager.instance.sunPower > 40)
            sr.sprite = lights[3];
        else if (GameManager.instance.sunPower > 30)
            sr.sprite = lights[2];
        else if (GameManager.instance.sunPower > 20)
            sr.sprite = lights[1];
        else if (GameManager.instance.sunPower > 10)
            sr.sprite = lights[0];
    }
}

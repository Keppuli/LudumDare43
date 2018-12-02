using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : MonoBehaviour {
    public static Priest instance = null; // Singleton

    Animator anim;
    void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "spearman")
        {
            anim.SetTrigger("Die");
            AudioManager.instance.PlayPriestDie();
            GameManager.instance.EndGame();
            AudioManager.instance.Stop();
        }

        if (collision.gameObject.name == "WinTrigger")
            RestartGame();
    }

    void RestartGame()
    {
        GameManager.instance.RestartGame();
    }
    public void CheckPoints()
    {
        if (GameManager.instance.sunPower >= 100)
        {
            anim.SetTrigger("Escape");
            AudioManager.instance.PlayPriestEscape();
            GameManager.instance.EndGame();
            AudioManager.instance.Stop();

        }
    }
}

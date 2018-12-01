using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager instance = null; // Singleton

    public GameObject buttonUpMaster;
    public GameObject buttonDownMaster;
    public GameObject buttonLeftMaster;
    public GameObject buttonRightMaster;

    void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        buttonUpMaster.SetActive(false);
        buttonDownMaster.SetActive(false);
        buttonLeftMaster.SetActive(false);
        buttonRightMaster.SetActive(false);
    }

    public void ShowButtonsVertical()
    {
        buttonUpMaster.SetActive(true);
        buttonDownMaster.SetActive(true);
        buttonLeftMaster.SetActive(false);
        buttonRightMaster.SetActive(false);
    }
    public void HideButtonsVertical()
    {
        buttonUpMaster.SetActive(false);
        buttonDownMaster.SetActive(false);
    }
    public void ShowLeftButton()
    {
        buttonLeftMaster.SetActive(true);
    }
    public void ShowRightButton()
    {
        buttonRightMaster.SetActive(true);
    }
}

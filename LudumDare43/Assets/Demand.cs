using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demand : MonoBehaviour {
    public static Demand instance = null; // Singleton

    public GameObject[] positionOBJs;
    //public int[] positionTypes;

    public Sprite man;
    public Sprite woman;
    public Sprite child;
    void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    private void Update()
    {
        Animate();
        CheckIfComplete();
    }
    void Start () {
        NewDemand();
	}
    void Animate()
    {
        for (int i = 0; i < positionOBJs.Length; i++)
        {
            if (positionOBJs[i].GetComponent<DemandPos>().status == DemandPos.Status.Open)
            {
                positionOBJs[i].GetComponent<Animator>().SetBool("Idle", false);
                break;
            }
        }
    }
    void CheckIfComplete()
    {
        int count = 0;
        for (int i = 0; i < positionOBJs.Length; i++)
        {
            if (positionOBJs[i].GetComponent<DemandPos>().status != DemandPos.Status.Open)
                count += 1;
        }
        if (count == 3)
        {
            CameraManager.instance.GetComponent<CameraManager>().shakeDuration = 2f;
            CameraManager.instance.GetComponent<CameraManager>().shakeAmount = 10f;
            AudioManager.instance.PlayGrowl();
            for (int i = 0; i < positionOBJs.Length; i++)
            {
                if (positionOBJs[i].GetComponent<DemandPos>().status == DemandPos.Status.Filled)
                {
                    // Give sun power
                    GameManager.instance.AddSunPower(positionOBJs[i].GetComponent<DemandPos>().CalculatePower());
                    
                }

            }
            NewDemand();
        }
    }


    public void FailDemand(GameObject demandPos)
    {
        demandPos.GetComponent<DemandPos>().DemandFailed();
    }

    public void FillDemand(GameObject demandPos)
    {
        demandPos.GetComponent<DemandPos>().DemandMet();
    }
  
    public bool CheckDemand(GameObject citizen)
    {
        int type = 0; // Default
        if (citizen.GetComponent<Citizen>().type == Citizen.Type.Man)
            type = 1;
        else if (citizen.GetComponent<Citizen>().type == Citizen.Type.Woman)
            type = 2;
        else if (citizen.GetComponent<Citizen>().type == Citizen.Type.Child)
            type = 3;
        
        for (int i = 0; i < positionOBJs.Length; i++)
        {
            // Check first position with no demands met yet
            if (positionOBJs[i].GetComponent<DemandPos>().status == DemandPos.Status.Open)
            {
                // Check if type fits
                if (CheckMatch(positionOBJs[i], type))
                {
                    FillDemand(positionOBJs[i]);
                    return true;
                }
                else
                {
                    FailDemand(positionOBJs[i]);
                    return false;
                }
            }
        }
        return false;
    }
    bool CheckMatch(GameObject posObj, int type)
    {
        if (type == 1)
            if (posObj.GetComponent<DemandPos>().type == DemandPos.Type.Man) // Check match
                return true;
        if (type == 2)
            if (posObj.GetComponent<DemandPos>().type == DemandPos.Type.Woman) // Check match
                return true;
        if (type == 3)
            if (posObj.GetComponent<DemandPos>().type == DemandPos.Type.Child) // Check match
                return true;
        // Default
        return false;
    }
    public void NewDemand()
    {
        for (int i = 0; i < positionOBJs.Length; i++)
        {
            int rnd = Random.Range(1, 4);
            if (rnd == 1)
                positionOBJs[i].GetComponent<DemandPos>().ChangeDemandType(DemandPos.Type.Man);
            else if (rnd == 2)
                positionOBJs[i].GetComponent<DemandPos>().ChangeDemandType(DemandPos.Type.Woman);
            else if (rnd == 3)
                positionOBJs[i].GetComponent<DemandPos>().ChangeDemandType(DemandPos.Type.Child);
            

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demand : MonoBehaviour {

    public GameObject[] positions;
    public int[] positionTypes;

    public Sprite man;
    public Sprite woman;
    public Sprite child;

    void Start () {
        NewDemand();
	}

    public void CompleteDemand()
    {

    }
    public void FillDemand(GameObject citizen)
    {
        if (citizen.GetComponent<Citizen>().type == Citizen.Type.Man)

        for (int i = 0; i < positions.Length; i++)
        {
            if (positionTypes[i] == 1)
            {
                positionTypes[i] = 0;
                break;
            }
        }
    }
    public void NewDemand()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            positionTypes[i] = Random.Range(1, 4);
            if (positionTypes[i] == 1)
                positions[i].GetComponent<SpriteRenderer>().sprite = man;
            else if (positionTypes[i] == 2)
                positions[i].GetComponent<SpriteRenderer>().sprite = woman;
            else if (positionTypes[i] == 3)
                positions[i].GetComponent<SpriteRenderer>().sprite = child;
        }
    }
    public void FailDemand()
    {

    }
}

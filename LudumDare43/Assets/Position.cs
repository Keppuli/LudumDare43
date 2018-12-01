using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour {

    [SerializeField]
    bool taken = false;

    public void TakePosition()
    {
        taken = true;
    }
    public void ReleasePosition()
    {
        taken = false;
    }
    public bool CheckStatus()
    {
        return taken;
    }
}

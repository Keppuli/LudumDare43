using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour {

    private void Destroy()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}

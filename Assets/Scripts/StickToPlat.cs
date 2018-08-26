using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToPlat : MonoBehaviour {

    public Transform plat1;
    public Transform plat2;
    public Transform plat3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lock")
        {
            transform.parent = plat1;
        }
        if (other.tag == "Lock2")
        {
            transform.parent = plat2;
        }
        if (other.tag == "Lock3")
        {
            transform.parent = plat3;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Lock")
        {
            transform.parent = null;
        }
        if (other.tag == "Lock2")
        {
            transform.parent = null;
        }
        if (other.tag == "Lock3")
        {
            transform.parent = null;
        }
    }
}

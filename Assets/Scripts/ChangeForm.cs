using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeForm : MonoBehaviour {

    public GameObject bigMode;
    public GameObject littleMode;

    private RaycastDetection rayDet;

    // Use this for initialization
    void Start () {
        rayDet = FindObjectOfType<RaycastDetection>();
	}
	
	// Update is called once per frame
	void Update () {
		if (rayDet.InShadow == true)
        {
            bigMode.SetActive(false);
            littleMode.SetActive(true);
        }
        else
        {
            bigMode.SetActive(true);
            littleMode.SetActive(false);
        }
	}
}

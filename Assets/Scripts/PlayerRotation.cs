using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRotation : MonoBehaviour {

    Quaternion desiredRotation;
    bool isTurning;
    public float rotationSpeed;
    private float turnRatio;

    private Quaternion startRotation;

    // Use this for initialization
    void Start ()
    {
        isTurning = false;
	}
	// Update is called once per frame
	void Update ()
    {
        // Debug.Log("Desired rotation: " + desiredRotation.eulerAngles);
        // Debug.Log("Current rotation: " + transform.rotation.eulerAngles);
        // Debug.Log("Current local rotation: " + transform.localEulerAngles);

        if (Input.GetAxisRaw("RHorizontal") > 0 && !isTurning)
        {
            //set desired rotation
            desiredRotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            //desiredRotation.eulerAngles = transform.eulerAngles + transform.up * 90;//new Vector3(0, 90, 0);
            //desiredRotation = Quaternion.LookRotation(Vector3.right, Vector3.zero);
            startRotation = transform.rotation;
            turnRatio = 0;
            isTurning = true;
        }
        //left on the right stick rotates camera left
        if (Input.GetAxisRaw("RHorizontal") < 0 && !isTurning)
        {
            //set desired rotation
            desiredRotation = transform.rotation * Quaternion.Euler(0, -90, 0);
            //desiredRotation.eulerAngles = transform.eulerAngles - transform.up * 90;//new Vector3(0, 90, 0);
            //desiredRotation.eulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - 90, transform.localEulerAngles.z);
            //desiredRotation = Quaternion.LookRotation(-Vector3.right, Vector3.zero);
            turnRatio = 0;
            startRotation = transform.rotation;
            isTurning = true;
        }

        if (isTurning)
        {
            turnRatio += Time.deltaTime / rotationSpeed;
            //slerp the rotation to desired rotation
            transform.rotation = Quaternion.Slerp(startRotation, desiredRotation, turnRatio);
            //if the distance from desired to start is minor
            if (turnRatio >= 1.0f)
            {
                //stop turning
                transform.rotation = desiredRotation;
                isTurning = false;
            }
        }
    }
}

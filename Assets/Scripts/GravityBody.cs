using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour {

    private float wallDetection = 0.6f;

    private Rigidbody rb;
    private GravityAttractor gravityAttractor;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        WallDetect();

        gravityAttractor.Attract(rb);
    }

    private void WallDetect()
    {
        RaycastHit objectHit;

        //Up
        if (Physics.Raycast(transform.position, transform.up, out objectHit, 0.5f))
        {
            if (objectHit.transform.tag == "Wall")
            {
                gravityAttractor = objectHit.transform.GetComponent<GravityAttractor>();
            }
        }

        //Forward
        else if (Physics.Raycast(transform.position, transform.forward, out objectHit, wallDetection))
        {
            if (objectHit.transform.tag == "Wall")
            {
                gravityAttractor = objectHit.transform.GetComponent<GravityAttractor>();
            }
        }

        //Right
        else if (Physics.Raycast(transform.position, transform.right, out objectHit, wallDetection))
        {
            if (objectHit.transform.tag == "Wall")
            {
                gravityAttractor = objectHit.transform.GetComponent<GravityAttractor>();
            }
        }

        //Back
        else if (Physics.Raycast(transform.position, -transform.forward, out objectHit, wallDetection))
        {
            if (objectHit.transform.tag == "Wall")
            {
                gravityAttractor = objectHit.transform.GetComponent<GravityAttractor>();
            }
        }

        //Left
        else if (Physics.Raycast(transform.position, -transform.right, out objectHit, wallDetection))
        {
            if (objectHit.transform.tag == "Wall")
            {
                gravityAttractor = objectHit.transform.GetComponent<GravityAttractor>();
            }
        }
        //Down
        else if (Physics.Raycast(transform.position, -transform.up, out objectHit, wallDetection))
        {
            if (objectHit.transform.tag == "Wall")
            {
                gravityAttractor = objectHit.transform.GetComponent<GravityAttractor>();
            }
        }
    }
}

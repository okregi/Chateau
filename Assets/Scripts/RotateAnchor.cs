using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnchor : MonoBehaviour
{
    [Tooltip("Rotation speed of anchor")]
    [SerializeField]
    private float rotSpeed = 1.0f;

    private PlayerMovement playerMovement;
    private RaycastDetection raycastDetection;
    private bool rotate = false;
    private float rotateTimer = 0.0f;
    private Quaternion oldRot;
    private Quaternion targetRot;
    private GameObject[] leftCornerList;
    private GameObject[] rightCornerList;


    // Use this for initialization
    void Awake ()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        raycastDetection = GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastDetection>();
        leftCornerList = GameObject.FindGameObjectsWithTag("LeftCorner");
        rightCornerList = GameObject.FindGameObjectsWithTag("RightCorner");
    }
	
	// Update is called once per frame
	void Update ()
    {
        Rotate();
    }

    private void Rotate()
    {
        if(playerMovement.LeftCorner)
        {

            if(Input.GetButtonDown("Fire1") && raycastDetection.InShadow && !playerMovement.OnGround)
            {
                //Start rotation
                playerMovement.CanMove = false;
                rotateTimer = 0.0f;
                oldRot = transform.rotation;
                targetRot = Quaternion.LookRotation(transform.right, new Vector3(0, 90.0f, 0));
                rotate = true;
            }
        }

        if (playerMovement.RightCorner)
        {
        
            if (Input.GetButtonDown("Fire1") && raycastDetection.InShadow && !playerMovement.OnGround)
            {
                //Start rotation
                playerMovement.CanMove = false;
                rotateTimer = 0.0f;
                oldRot = transform.rotation;
                targetRot = Quaternion.LookRotation(transform.right, new Vector3(0, -90.0f, 0));
                rotate = true;
            }
        }

        if (rotate == true)
        {
            //Deactivate right corner triggers
            for(int i = 0; i < rightCornerList.Length; i++)
            {
                rightCornerList[i].SetActive(false);
            }

            //Deactivate left corner triggers
            for (int c = 0; c < leftCornerList.Length; c++)
            {
                leftCornerList[c].SetActive(false);
            }

            rotateTimer += Time.deltaTime;

            if (transform.rotation != targetRot) /* Does not equal our target rotation */
            {
                //Slerp towards rotation
                transform.rotation = Quaternion.Slerp(oldRot, targetRot, rotateTimer * rotSpeed);
            }
        }


        if (rotateTimer > 1.0f)
        {
            //Reset
            rotate = false;
            playerMovement.LeftCorner = false;
            playerMovement.RightCorner = false;
            transform.rotation = targetRot;
            playerMovement.gameObject.transform.parent = null;
            playerMovement.AnchorCount = 0;

            //Activate right corner triggers
            for (int i = 0; i < rightCornerList.Length; i++)
            {
                rightCornerList[i].SetActive(true);
            }

            //Activate left corner triggers
            for (int c = 0; c < leftCornerList.Length; c++)
            {
                leftCornerList[c].SetActive(true);
            }

            Destroy(gameObject);
            rotateTimer = 0.0f;
        }
    }
}

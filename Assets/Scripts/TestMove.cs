using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour {
    #region Inspector vars
    [SerializeField] private float walkSpeed = 6;
    [SerializeField] private float wallDetection = 0.5f;
    [SerializeField] private float rotSpeed = 1.0f;
    #endregion

    #region Private vars
    private Vector3 moveAmount;
    private Vector3 smoothMoveVelocity;
    private float verticalLookRotation;
    private Rigidbody rb;
    private Vector3 direction = Vector3.zero;
    private bool rotate = false;
    private Quaternion oldRot;
    private Quaternion targetRot;
    private float rotTimer;
    private RaycastDetection raycastDetection;
    private bool detectFloor = false;
    private bool canMove = true;
    private float controlStaticTimer;
    private float inputX;
    private float inputY;
    private bool onWall = false;
    #endregion

    #region Get Set
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }
    #endregion


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Raycasting to detect wall
        DetectWalls();

        //Rotation across walls
        RotatePlayer();

        if (canMove)
        {
            controlStaticTimer = 0.0f;

            //Calculate movement
            inputX = Input.GetAxisRaw("Horizontal");
            inputY = Input.GetAxisRaw("Vertical");
            raycastDetection = GetComponent<RaycastDetection>();

            Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
            Vector3 targetMoveAmount = moveDir * walkSpeed;
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
        }
        else
        {
            inputX = 0.0f;
            inputY = 0.0f;
            moveAmount = Vector3.zero;
            controlStaticTimer += Time.deltaTime;
        }

        if (onWall)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.useGravity = true;
        }

        if (controlStaticTimer > 1.2f)
        {
            canMove = true;
        }

        #region Old Camera rotation 
        ////Look rotation (option for controller or mouse)
        //if (!controller)
        //{
        //    transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
        //    verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        //}
        //else
        //{
        //    transform.Rotate(Vector3.up * Input.GetAxis("RHorizontal") * mouseSensitivityX);
        //    verticalLookRotation += Input.GetAxis("RVertical") * mouseSensitivityY;
        //}
        //
        //verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        //cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;
        #endregion
    }

    private void DetectWalls()
    {
        RaycastHit objectHit;

        //Shoot raycast in four directions

        //Forward
        if (Physics.Raycast(transform.position, transform.forward, out objectHit, wallDetection))
        {
            Debug.Log("Forward Raycast on: " + objectHit.collider);
            Debug.DrawRay(transform.position, transform.forward, Color.blue);

            if (objectHit.transform.tag == "Wall")
            {
                direction = objectHit.normal.normalized;
                detectFloor = false;
            }
            else if (objectHit.transform.tag == "Floor")
            {
                direction = objectHit.normal.normalized;
                detectFloor = true;
            }
        }

        //Right
        else if (Physics.Raycast(transform.position, transform.right, out objectHit, wallDetection))
        {
            Debug.Log("Right Raycast on: " + objectHit.collider);
            Debug.DrawRay(transform.position, transform.right, Color.blue);

            if (objectHit.transform.tag == "Wall")
            {
                direction = objectHit.normal.normalized;
                detectFloor = false;
            }
            else if (objectHit.transform.tag == "Floor")
            {
                direction = objectHit.normal.normalized;
                detectFloor = true;
            }
        }

        //Back
        else if (Physics.Raycast(transform.position, -transform.forward, out objectHit, wallDetection))
        {
            Debug.Log("Back Raycast on: " + objectHit.collider);
            Debug.DrawRay(transform.position, -transform.forward, Color.blue);

            if (objectHit.transform.tag == "Wall")
            {
                direction = objectHit.normal.normalized;
                detectFloor = false;
            }
            else if (objectHit.transform.tag == "Floor")
            {
                direction = objectHit.normal.normalized;
                detectFloor = true;
            }
        }

        //Left
        else if (Physics.Raycast(transform.position, -transform.right, out objectHit, wallDetection))
        {
            Debug.Log("Left Raycast on: " + objectHit.collider);
            Debug.DrawRay(transform.position, -transform.right, Color.blue);

            if (objectHit.transform.tag == "Wall")
            {
                direction = objectHit.normal.normalized;
                detectFloor = false;
            }
            else if (objectHit.transform.tag == "Floor")
            {
                direction = objectHit.normal.normalized;
                detectFloor = true;
            }
        }
        else
        {
            direction = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        //Apply movement to rigidbody
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + localMove);
    }

    private void RotatePlayer()
    {
        // Leaving Wall
        if (Input.GetButtonDown("Fire1") && direction != Vector3.zero && detectFloor == true)
        {
            canMove = false;
            rotTimer = 0.0f;
            oldRot = transform.rotation;
            //targetRot = Quaternion.LookRotation(transform.forward, direction);
            //targetRot = Quaternion.LookRotation(-Vector3.right, direction);

            // Looking intp floor
            if (Vector3.Dot(transform.forward, direction) < -0.95f)
            {
                targetRot = Quaternion.LookRotation(transform.up, Vector3.up);
            }
            // Looking Away From floor
            if (Vector3.Dot(transform.forward, direction) > 0.95f)
            {
                targetRot = Quaternion.LookRotation(-transform.up, Vector3.up);
            }

            // Looking Across the floor
            if (Vector3.Dot(transform.forward, direction) < 0.05f &&
                Vector3.Dot(transform.forward, direction) > -0.05f)
            {
                targetRot = Quaternion.LookRotation(transform.forward, Vector3.up);
            }

            rotate = true;
            direction = Vector3.zero;
            onWall = false;
        }
        // Mounting wall
        else if (Input.GetButtonDown("Fire1") && direction != Vector3.zero && raycastDetection.InShadow == true)
        {

            canMove = false;
            onWall = true;
            rotTimer = 0.0f;
            oldRot = transform.rotation;

            // Looking intp wall
            if( Vector3.Dot(transform.forward, direction) < -0.95f)
            {
                targetRot = Quaternion.LookRotation(Vector3.up, direction);
            }
            // Looking Away From Wal
            if (Vector3.Dot(transform.forward, direction) > 0.95f)
            {
                targetRot = Quaternion.LookRotation(Vector3.down, direction);
            }

            // Looking Across the wall
            if (Vector3.Dot(transform.forward, direction) < 0.05f &&
                Vector3.Dot(transform.forward, direction) > -0.05f)
                {
                targetRot = Quaternion.LookRotation(transform.forward, direction);
            }

                        //targetRot = Quaternion.LookRotation(Vector3.up, direction);
            rotate = true;
            direction = Vector3.zero;
        }


        if (rotate == true)
        {
            rotTimer += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(oldRot, targetRot, rotTimer * rotSpeed);
        }

        if (rotTimer > 1)
        {
            rotate = false;
            rotTimer = 0;
            transform.rotation = targetRot;
        }
    }
}

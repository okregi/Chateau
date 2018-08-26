using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDetection : MonoBehaviour
{
    #region Inspector vars
    [Tooltip("The range between object and light")]
    [SerializeField]
    private float range = 10.0f;
    [Tooltip("The distance the rays are from the origin object")]
    [SerializeField]
    float distanceFromStart = 0.23f;
    [Tooltip("The max amount of rays needed to fall")]
    [SerializeField]
    private int maxRays = 3;
    #endregion

    #region Private vars
    private bool[] rayHits;
    private List<Vector3> lightDirections;
    private Vector3[] startPos;
    private bool inShadow;
    private int inLightCount;
    #endregion

    #region Get Set

    public bool InShadow
    {
        get { return inShadow; }
    }

    #endregion

    private void Start()
    {
        startPos = new Vector3[6];

        Light[] lights = FindObjectsOfType<Light>();

        rayHits = new bool[startPos.Length];
        lightDirections = new List<Vector3>();

        //Set up light directions
        for (int i = 0; i < lights.Length; i++)
        {
            lightDirections.Add(-lights[i].transform.forward);
        }
    }

    private void Update()
    {
        inLightCount = 0;
        inShadow = true; 

        startPos[0] = transform.position + transform.forward * distanceFromStart;   //Front
        startPos[1] = transform.position + transform.right * distanceFromStart;     //Right
        startPos[2] = transform.position + -transform.forward * distanceFromStart;  //Back
        startPos[3] = transform.position + -transform.right * distanceFromStart;    //Left
        startPos[4] = transform.position + transform.up * distanceFromStart;        //Top
        startPos[5] = transform.position + -transform.up * distanceFromStart;       //Bottom


        //Iterate through and raycast towards the light
        for (int l = 0; l < lightDirections.Count; l++)
        {
            for (int i = 0; i < startPos.Length; i++)
            {
                Ray ray = new Ray(startPos[i], lightDirections[l]);
                rayHits[i] = Physics.Raycast(ray);

                //Check if we have hit anything
                if (rayHits[i])
                {
                    Debug.DrawRay(startPos[i], lightDirections[l] * range, Color.red);
                }
                else
                {
                    inLightCount++; 
                    Debug.DrawRay(startPos[i], lightDirections[l] * range, Color.green);
                }
            }
        }

        //Check whether or not we are in the shadow
        inShadow = inLightCount <= maxRays;
    }
}

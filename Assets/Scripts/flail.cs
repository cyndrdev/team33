using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flail : MonoBehaviour
{
    //the object to move
    public Transform flailHead;
    public float flailSpeed, timeForFlailToStop, timeForFlailGrappleToStop, grappleSpeed, rotationSpeed;
    private float timeForFlailGrappleToStopTemp, timeForFlailToStopTemp;
    public bool Throw, grapple;
    public PlayerMovement plMovement;
    // Start is called before the first frame update
    void Start()
    {
        timeForFlailGrappleToStopTemp = timeForFlailGrappleToStop;
        timeForFlailToStopTemp = timeForFlailToStop;
    }
     void Update()
     {
        //  didn't figure this out yet it should let it rotate around the player but just acted very weird need a bit more time to get it to work
        // flailHead.RotateAround(transform.position, new Vector3(1, 1, 0), rotationSpeed * Time.deltaTime);
        if (Input.GetMouseButton(0))
        {
            Throw = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            grapple = true;
        }
        if (Throw && timeForFlailToStopTemp > 0)
        {
            flailThrow();
            timeForFlailToStopTemp -= Time.deltaTime;
        }
        if (timeForFlailToStopTemp <=0)
        {
            timeForFlailToStopTemp = timeForFlailToStop;
            Throw = false;
            plMovement.slowDown = false;
        }
        if (grapple && timeForFlailGrappleToStopTemp > 0)
        {
            grappleThrow();
            timeForFlailGrappleToStopTemp -= Time.deltaTime;
        }
        if (timeForFlailGrappleToStopTemp <= 0)
        {
            timeForFlailGrappleToStopTemp = timeForFlailGrappleToStop;
            grapple = false;
            plMovement.slowDown = false;
        }
     }
    private void grappleThrow()
    {
        plMovement.slowDown = true;
        transform.position = Vector3.MoveTowards(transform.position, flailHead.position, grappleSpeed * Time.deltaTime);
        flailHead.parent = null;
    }
    //this is where the flail movement code happens
    private void flailThrow()
    {
        plMovement.slowDown = true;
        var pos = Input.mousePosition;
        pos.z = 45;
        pos = Camera.main.ScreenToWorldPoint(pos);
        flailHead.position = Vector3.MoveTowards(flailHead.position, pos, flailSpeed * Time.deltaTime);
    }
}

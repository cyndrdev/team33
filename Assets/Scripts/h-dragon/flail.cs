using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flail : MonoBehaviour
{
    //the object to move
    public Transform flailHead;
    public float flailSpeed, timeForFlailToStop, timeForFlailGrappleToStop, grappleSpeed, rotationSpeed;
    private float timeForFlailGrappleToStopTemp, timeForFlailToStopTemp;
    public bool Throw, grapple, orbit;
    public PlayerMovement plMovement;
    public float orbitDistance = 10.0f;
    public float orbitDegreesPerSec = 180.0f;
    public LayerMask layerToIgnore;
    public DistanceJoint2D dsJoint;
    public LineRenderer lineRender;
    // Start is called before the first frame update
    void Start()
    {
        timeForFlailGrappleToStopTemp = timeForFlailGrappleToStop;
        timeForFlailToStopTemp = timeForFlailToStop;
    }
    private void OnCollisionEnter(Collision collision)
    {
    	if (collision.gameObject.layer == layerToIgnore)
        Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    }
     void Update()
     {
        lineRender.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0));
        lineRender.SetPosition(1, new Vector3(flailHead.position.x, flailHead.position.y, 0));
        if (Input.GetMouseButton(0) && !Throw && !grapple)
        {
            orbit =true;
        }
        else
        {
            dsJoint.enabled = true;
            orbit =false;
        }
        if (orbit)
        {
            Orbit();
        }
        // flailHead.RotateAround(transform.position, new Vector3(x, y, z), rotationSpeed * Time.deltaTime);
        if (Input.GetMouseButtonUp(0))
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
    void Orbit()
    {
        dsJoint.enabled = false;
        // Keep us at orbitDistance from target
        flailHead.position = transform.position + (flailHead.position - transform.position).normalized * orbitDistance;
        flailHead.RotateAround(transform.position, new Vector3(0, 0, 1), orbitDegreesPerSec * Time.deltaTime);
    }
    private void grappleThrow()
    {
        dsJoint.enabled = false;
        plMovement.slowDown = true;
        transform.position = Vector3.MoveTowards(transform.position, flailHead.position, grappleSpeed * Time.deltaTime);
        flailHead.parent = null;
    }
    //this is where the flail movement code happens
    private void flailThrow()
    {
        dsJoint.enabled = false;
        plMovement.slowDown = true;
        var pos = Input.mousePosition;
        pos.z = 45;
        pos = Camera.main.ScreenToWorldPoint(pos);
        flailHead.position = Vector3.MoveTowards(flailHead.position, pos, flailSpeed * Time.deltaTime);
    }
}

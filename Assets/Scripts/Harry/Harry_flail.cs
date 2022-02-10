using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Harry_flail : MonoBehaviour
{
    //head moves (slow ish) towards mouse click if mouse click within range
    //plays animation and screen shake on impact.
    //ability for player to shoot body towards head
    //ability for player to shoot head towards body

    [SerializeField] float range;
    [SerializeField] float attackTime;
    [SerializeField] float pullBackTime;
    [SerializeField] float scaleChange;
    float t;

    [SerializeField] GameObject Player;

    bool Attacking;
    bool FlailOut;

    Vector2 target;
    Vector2 StartPos;

    Vector3 defaultScale;

    [SerializeField] LineRenderer lr;

    private void Update()
    {
        //if the player click mouse button and where they clicked is in range or attacking already
        lr.SetPositions(new Vector3[] { Player.transform.position, transform.position });
        if (((Input.GetKeyDown(KeyCode.Mouse0) && Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), Player.transform.position) <= range && Attacking == false) | Attacking) && !FlailOut)
        {
            
            Attacking = true;
            if (target == Vector2.zero)
            {
                defaultScale = transform.localScale;
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                StartPos = transform.position;
            }
            //Moves the flail to the mouse pointer in attack time seconds
            //Maybe doing it in a set time is bad because it makes short range attacks look slow and long range attacks look fast
            //this makes short range attacks more powerful
            t += Time.deltaTime / attackTime;
            transform.position = Vector2.Lerp(StartPos, target, t);
            //increases and decreases the scale of the flail so it looks like it went up and down
            if(Vector2.Distance(transform.position, target) < Vector2.Distance(StartPos, target) / 2)
            {
                transform.localScale *= 1 - scaleChange;
            }
            else
            {
                transform.localScale *= 1 + scaleChange;
            }
            //when the flail is in the right position
            if(Vector2.Distance(transform.position, target) < 0.1f)
            {
                Attacking = false;
                //resets the target (if you click 0,0 it still works)
                target = Vector2.zero;
                t = 0;
                //resets the flail scale
                transform.localScale = defaultScale;
                FlailOut = true;
                //makes it so the flail doesn't move till you pull it back in
                GetComponent<Rigidbody2D>().mass = 10000f;
                //gives some camera shake for impact
                CameraShaker.Instance.ShakeOnce(3f, 1f, 0.1f, 0.1f);
            }
        }
        else if (FlailOut && Input.GetKeyDown(KeyCode.Mouse0) && !Attacking)
        {
            StartCoroutine(Pullback());
        }
        else if (FlailOut && Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine(PlayerPull());
        }
        if(Input.GetKeyDown(KeyCode.Space) && !FlailOut)
        {
            StartCoroutine(SpinAttack());
        }
    }

    IEnumerator Pullback()
    {
        //gives camera shake for impact
        CameraShaker.Instance.ShakeOnce(3f, 0.5f, 0.1f, 0.1f);
        StartPos = transform.position;
        //moves the flail back to the player
        while ((Vector2.Distance(transform.position, Player.transform.position) > 0.2f))
        {
            t += Time.deltaTime / pullBackTime;
            transform.position = Vector2.Lerp(StartPos, Player.transform.position, t);
            yield return new WaitForEndOfFrame();
        }
        FlailOut = false;
        t = 0;
        //makes it so the flail can move again
        GetComponent<Rigidbody2D>().mass = 1.5f;
    }

    IEnumerator PlayerPull()
    {
        //gives camera shake for impact
        //CameraShaker.Instance.ShakeOnce(3f, 0.5f, 0.1f, 0.1f);
        StartPos = Player. transform.position;
        //moves the player towards the player
        while ((Vector2.Distance(transform.position, Player.transform.position) > 0.2f))
        {
            t += Time.deltaTime / pullBackTime;
            Player.transform.position = Vector2.Lerp(StartPos, transform.position, t);
            yield return new WaitForEndOfFrame();
        }
        FlailOut = false;
        t = 0;
        //makes it so the flail can move again
        GetComponent<Rigidbody2D>().mass = 1.5f;
    }

    IEnumerator SpinAttack()
    {
        Player.GetComponent<DistanceJoint2D>().maxDistanceOnly = false;
        Attacking = true;
        FlailOut = true;
        GetComponent<Rigidbody2D>().mass = 10000f;
        float i = 3f;
        while(i > 0)
        {
            CameraShaker.Instance.ShakeOnce(3f, 0.25f, 0.1f, 0.1f);
            i -= Time.deltaTime;
            transform.RotateAround(Player.transform.position, Vector3.forward, 500f * Time.deltaTime);
            //transform.position += transform.position - Player.transform.position * Time.deltaTime * 0.00000000001f;
            yield return new WaitForEndOfFrame();
        }
        GetComponent<Rigidbody2D>().mass = 1.5f;
        FlailOut = false;
        Attacking = false;
        Player.GetComponent<DistanceJoint2D>().maxDistanceOnly = true;
    }
}
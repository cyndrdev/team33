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
        if ((Input.GetKeyDown(KeyCode.Mouse0) && Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), Player.transform.position) <= range && Attacking == false) | Attacking && !FlailOut)
        {
            
            Attacking = true;
            if (target == Vector2.zero)
            {
                defaultScale = transform.localScale;
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                StartPos = transform.position;
            }
            //Moves the flail to the mouse pointer in attack time seconds
            t += Time.deltaTime / attackTime;
            transform.position = Vector2.Lerp(StartPos, target, t);
            //increases and decreases the scale of the flail so it looks like it went up and down
            if(Vector2.Distance(transform.position, target) < Vector2.Distance(StartPos, target) / 2)
            {
                transform.localScale *= 0.99f;
            }
            else
            {
                transform.localScale *= 1.01f;
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
        else if (FlailOut && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Pullback());
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
}
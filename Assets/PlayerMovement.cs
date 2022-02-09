using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Stores player input data for their movement
    [SerializeField] Vector2 Movement;

    //Stores the speed of the player
    [SerializeField] float Speed;
    //If you ever need to stop movement just make this true
    public bool dontMove;

    // Update is called once per frame
    void Update()
    {
        //sets the movement x and y to the player input axis. This allows WASD and Arrow key movement
        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");

        //Slows diagonal movement, which improves game feel because otherwise moving diagonal is twice as fast as moving forwards
        if(Mathf.Abs(Movement.x) == 1 && Mathf.Abs(Movement.y) == 1)
        {
            Movement *= 0.75f;
        }
        if (!dontMove)
        {   
            transform.position += (Vector3)Movement * Speed * Time.deltaTime;
            //this is because the object might go in the z corridinates which will not let the camera seed iffernt things as they might get behind the camera if they are a child of the moving object
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
}

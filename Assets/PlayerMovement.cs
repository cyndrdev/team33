using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Stores player input data for their movement
    [SerializeField] Vector2 Movement;

    //Stores the speed of the player
    [SerializeField] float Speed;

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

        transform.position += (Vector3)Movement * Speed * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{

    Vector2 startPosition;
    Rigidbody2D rbody;
    public LayerMask player;
    public LayerMask ground;
    public LayerMask wall;
    bool yFrozen;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        startPosition = rbody.transform.position;
        yFrozen = false;
    }

    // Update is called once per frame
    void Update()
    {

        //makes the player unable to move the rock up and down or when walking on top of the rock
        if ((PlayerHit() && !HitWall()))
        {
            rbody.constraints = RigidbodyConstraints2D.None;
            rbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            yFrozen = true;
        }
        else if (!PlayerHit())
        {
            rbody.constraints = RigidbodyConstraints2D.None;
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
        else
        { 
            rbody.constraints = RigidbodyConstraints2D.None;
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    //code for resetting the rock
    public void ResetRock()
    {
        rbody.transform.position = startPosition;
        rbody.velocity = Vector2.zero;
        rbody.constraints = RigidbodyConstraints2D.None;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }

    //checks if player pushing the rock
    bool PlayerHit()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 3.0f, player);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, 3.0f, player);
        return ((leftHit.collider != null) || (rightHit.collider != null));
    }

    //checks if rock is against the wall
    bool HitWall()
    {
        RaycastHit2D hitLeftWall = Physics2D.Raycast(transform.position, Vector2.left, 3.0f, wall);
        RaycastHit2D hitRightWall = Physics2D.Raycast(transform.position, Vector2.left, 3.0f, wall);

        return ((hitLeftWall.collider != null) || (hitRightWall.collider != null));
    }
}

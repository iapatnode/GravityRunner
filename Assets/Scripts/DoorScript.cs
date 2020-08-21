using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isVertical;
    public bool isHidden;
    public bool moveOpposite;
    public float SPEED;
    public float distance;

    Rigidbody2D rbody;
    Vector2 startPosition;
    bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        startPosition = rbody.transform.position;
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(rbody.velocity != Vector2.zero)
        {
            if(isVertical)
            {
                CheckVerticalDoor();
            }
            else
            {
                CheckHorizontalDoor();
            }
        }
    }

    //resets the door
    public void ResetDoor()
    {
        isOpen = false;
        rbody.transform.position = startPosition;
        rbody.velocity = Vector2.zero;
    }

    //checks the various states the door could be in when moving vertical
    private void CheckVerticalDoor()
    {
        if ((!moveOpposite && isOpen && rbody.position.y <= startPosition.y - distance) ||
                (!moveOpposite && !isOpen && rbody.position.y >= startPosition.y) ||
                (moveOpposite && isOpen && rbody.position.y >= startPosition.y + distance) ||
                (moveOpposite && !isOpen && rbody.position.y <= startPosition.y))
        {
            rbody.velocity = new Vector2(0, 0);
        }
    }

    //checks the various states the door could be in when moving horizontal
    private void CheckHorizontalDoor()
    {

        if ((!moveOpposite && isOpen && rbody.position.x >= startPosition.x + rbody.transform.localScale.x)
                || (!moveOpposite && !isOpen && rbody.position.x <= startPosition.x)
                || (moveOpposite && isOpen && rbody.position.x <= startPosition.x - rbody.transform.localScale.x)
                || (moveOpposite && !isOpen && rbody.position.x >= startPosition.x))
        {
            rbody.velocity = new Vector2(0, 0);
        }
    }

    //opens the door when called
    public void OpenDoor()
    {
        if(isVertical)
        {
            OpenVerticalDoor();
        }
        else
        {
            OpenHorizontalDoor();
        }

        isOpen = !isOpen;
    }

    //opens vertical moving doors
    private void OpenVerticalDoor()
    {
        if ((!isOpen && !moveOpposite) || (isOpen && moveOpposite))
        {
            rbody.velocity = new Vector2(0, -SPEED);
        }
        else
        {
            rbody.velocity = new Vector2(0, SPEED);
        }
    }

    //opens horizontal moving doors
    private void OpenHorizontalDoor()
    {
        if ((!isOpen && !moveOpposite) || (isOpen && moveOpposite))
        {
            rbody.velocity = new Vector2(SPEED, 0);
        }
        else
        {
            rbody.velocity = new Vector2(-SPEED, 0);
        }
    }
}

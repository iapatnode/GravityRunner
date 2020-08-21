using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public DoorScript door;
    public float SPEED;
    Rigidbody2D rbody;
    Vector2 startPosition;
    bool isHit;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        startPosition = rbody.transform.position;
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rbody.velocity != Vector2.zero)
        {
            if (isHit && (rbody.position.y <= startPosition.y - 1) || (!isHit && rbody.position.y >= startPosition.y))
            {
                rbody.velocity = new Vector2(0, 0);
            }
        }
    }

    public void ResetButton()
    {
        door.ResetDoor();
        rbody.transform.position = startPosition;
        rbody.velocity = Vector2.zero;
        isHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Equals("Player") || collision.gameObject.name.Contains("Rock"))
        {
            MoveButton();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player") || collision.gameObject.name.Contains("Rock"))
        {
            MoveButton();
        }
    }
    
    public void MoveButton()
    {
        if(!isHit)
        {
            rbody.velocity = new Vector2(0, -SPEED);
        }
        else
        {
            rbody.velocity = new Vector2(0, SPEED);
        }

        isHit = !isHit;

        MoveDoor();
    }

    public void MoveDoor()
    {
        door.GetComponent<DoorScript>().OpenDoor();
    }
}

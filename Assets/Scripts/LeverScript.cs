using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public DoorScript door;
    Animator animator;
    bool hasMoved;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hasMoved = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveLever()
    {
        if(hasMoved)
        {
            animator.SetBool("Switch", false);
        }
        else
        {
            animator.SetBool("Switch", true);
        }

        hasMoved = !hasMoved;
    }

    public void MoveDoor()
    {
        door.OpenDoor();
    }

    public void ResetLever()
    {
        hasMoved = false;
        animator.SetBool("Switch", false);
        door.ResetDoor();
    }
}

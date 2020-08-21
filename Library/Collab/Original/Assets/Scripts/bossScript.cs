using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossScript : MonoBehaviour
{

    Rigidbody2D rbody;
    SpriteRenderer rend;
    Animator animator;

    int frequency;
    bool facingRight;
    bool spriteRight;
    bool upsideDown;
    bool alive;

    int attack;
    float health;
    int speed;

    public healthbarScript bar;
    System.Random rand;

    public GameObject player;
    public GameObject splat;


    int attackCooldown;
    void Start()
    {
        bar = FindObjectOfType<healthbarScript>();
        player = FindObjectOfType<PlayerScript>().gameObject;
        attackCooldown = 0;
        //Frequency is the frequency of our attacks
        frequency = 180;
        speed = 5;
        health = 1.0f;
        facingRight = false;
        spriteRight = false;
        upsideDown = false;
        alive = true;

        rand = new System.Random();

        rbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        bar.SetValue(1f);

    }



    void Update()
    {
        if (alive)
        {
            if (attackCooldown >= frequency)
            {
                if (attackCooldown == frequency)
                {
                    animator.SetBool("moving", false);
                    //Get the attack we will make based on player location
                    //NOTE: starts charging the attack animation
                    attack = getAttack();
                }
                //Charge attack
                if (attack == 0)
                {
                    //Dash for a bit
                    if (attackCooldown > frequency + 50)
                    {
                        dash();
                    }
                    //Continue dashing for 100, when done, reset atkcooldown and stop moving
                    if (attackCooldown > frequency + 100)
                    {
                        attackCooldown = 0;
                        animator.SetBool("moving", false);
                    }
                }
                //Gravity flip
                else
                {
                    //Flip ONCE
                    if (attackCooldown == frequency + 40)
                    {
                        GravityFlip();
                    }
                    //Wait a second before resuming
                    else if (attackCooldown > frequency + 100)
                    {
                        print("DONE");
                        attackCooldown = 0;
                        animator.SetBool("moving", false);
                    }
                    else
                    {
                        move();
                        facePlayer();
                    }
                }
            }
            //movement
            else if (attackCooldown > 80)
            {
                animator.SetBool("moving", true);
                move();
                facePlayer();
            }
            else if (attackCooldown < 80)
            {
                rbody.velocity = new Vector2(0, rbody.velocity.y);
                animator.SetBool("moving", false);
                facePlayer();
            }

            attackCooldown++;
        }
    }

    //Destroys enemy when hit with player's bullet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alive)
        {
            if (collision.name.Contains("PlayerBullet"))
            {
                Instantiate(splat, transform.position, Quaternion.identity);
                health -= 0.05f;
                bar.SetValue(health);
                //Gives the player points for killing an enemy
                //int pointValue = 3;
                //MSM.Instance.IncrementScore(pointValue);
                //other things that should happen
                if (health < 0)
                {
                    animator.Play("die");
                    alive = false;
                    gameObject.tag = "Goal";
                    bar.die();
                }
                //Critical mode!
                else if(health < 0.3f)
                {
                    speed = 7;
                    frequency = 140;
                    bar.goCritical();
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (alive)
        {
            if (collision.gameObject.name.Equals("Player"))
            {
                Invoke("resetBoss", 1);
            }
        }
    }

    //Changes the x direction the player faces
    private void FlipX()
    {
        rend.flipX = facingRight;
        spriteRight = !spriteRight;
    }

    //Flips the y direction of the player
    private void GravityFlip()
    {
        animator.SetBool("chargejump", false);
        animator.SetBool("moving", true);
        rbody.gravityScale *= -1;
        upsideDown = !upsideDown;
        rend.flipY = upsideDown;
    }

    //Determine which attack to use
    private int getAttack()
    {
        //If the player is on the ceiling
        if (player.transform.position.y > 0)
        {
            //If we are on the same level as the player
            if (upsideDown)
            {
                animator.SetBool("chargedash", true);
                return 0;
            }
            //If we are not on the same level as the player
            else
            {
                animator.SetBool("chargejump", true);
                return 1;
            }
        }
        //If the player is on the floor
        else
        {
            //If we are on the same level as the player
            if (!upsideDown)
            {
                animator.SetBool("chargedash", true);
                return 0;
            }
            //If we are not on the same level as the player
            else
            {
                animator.SetBool("chargejump", true);
                return 1;
            }
        }
    }

    private void facePlayer()
    {
        //If the player is to the left relative to the boss position
        if (player.transform.position.x < transform.position.x)
        {
            facingRight = false;
            print("LEFT");
        }
        //If the player is to the right relative to boss position
        else if (player.transform.position.x > transform.position.x)
        {
            facingRight = true;
            print("RIGHT");
        }
        print(transform.position.x > player.transform.position.x);
        if (facingRight != spriteRight)
        {
            FlipX();
        }

    }

    private void move()
    {
        if (facingRight)
        {
            rbody.velocity = new Vector2(speed, rbody.velocity.y);
        }
        else
        {
            rbody.velocity = new Vector2(-speed, rbody.velocity.y);
        }
    }


    private void dash()
    {
        animator.SetBool("chargedash", false);
        animator.SetBool("moving", true);
        if (facingRight)
        {
            rbody.velocity = new Vector2(35, rbody.velocity.y);
        }
        else
        {
            rbody.velocity = new Vector2(-35, rbody.velocity.y);
        }
    }

    private void resetBoss()
    {
        if (upsideDown)
        {
            gameObject.transform.position = new Vector2(21, 16);
        }
        else
        {
            gameObject.transform.position = new Vector2(21, -16);
        }
        attackCooldown = 0;
        animator.SetBool("chargedash", false);
        animator.SetBool("chargejump", false);
        animator.SetBool("moving", false);
        animator.Play("idle");
    }

}


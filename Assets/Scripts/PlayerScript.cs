using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rbody;
    SpriteRenderer rend;
    Animator ani;
    Transform trans;
    public BulletScript bulletPrototype;
    public CanvasScript canvas;
    public LayerMask ground;
    Vector3 currentScale;
    public const float BULLET_SPEED = 30;
    Vector2 startPosition;
    int cooldown;
    bool facingRight;
    bool upsideDown;
    bool alive;
    bool Hit;
    int signNumber;
    bool Gravity;
    bool grounded;
    objectPool bulletPool;
    Vector2 shootRight = new Vector2(2.5f, 0);
    Vector2 shootDown = new Vector2(0, -.3f);

    void Start()
    {
        alive = true;
        rbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        startPosition = rbody.position;
        cooldown = 0;
        facingRight = false;
        upsideDown = false;
        Hit = false;
        signNumber = 0;
        Gravity = true;
        grounded = true;
        bulletPool = new objectPool(bulletPrototype.gameObject, true, 15);
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(transform.position, 2.5f, ground);
        if (rbody.velocity.x != 0)
        {
            ani.SetBool("Moving", true);
        }
        else {
            ani.SetBool("Moving", false);
        }
        //while player is alive, they can play the level
        if (alive)
        {
            cooldown--;

            //Gets direction of player
            float direction = Input.GetAxis("Horizontal");

            //Changes direction of player
            if (direction < 0 && facingRight)
            {
                FlipX();
            }
            else if (direction > 0 && !facingRight)
            {
                FlipX();
            }

            //Flip gravity
            if (Input.GetKeyDown(KeyCode.Z) && grounded)
            {
                GravityFlip();
            }
            //Shoot
            if (Input.GetKeyDown(KeyCode.X) && cooldown < 0)
            {
                cooldown = 20;
                MSM.Instance.GunSound();
                SpawnPlayerBullets();
            }
        }
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            //Allows player to move in x direction if not dead
            rbody.velocity = new Vector2(15 * Input.GetAxis("Horizontal"), rbody.velocity.y);
        }
        else
        {
            rbody.velocity = new Vector2(0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (alive)
        {
            //Player loses life if he hits spike or falls too far
            if (collision.gameObject.tag.Equals("Ceiling") || collision.gameObject.tag.Equals("Spike") || collision.gameObject.tag.Equals("Quicksand") || collision.gameObject.tag.Equals("boss")||collision.gameObject.tag.Equals("Enemy"))
            {
                MSM.Instance.LostLife();
                rend.enabled = false;
                alive = false;

                Invoke("ResetPlayer", 1);
            }

            //Sends player to next scene when level is over
            if (collision.gameObject.tag.Equals("Goal"))
            {
                MSM.Instance.setScore();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroys normal coins and gives player necessary points
        if (collision.gameObject.tag.Equals("Coin"))
        {
            MSM.Instance.AddTime();
            Destroy(collision.gameObject);
        }

        //actions for if the player is hit by an enemy bullet
        if (collision.name.Contains("EnemyBullet"))
        {
            if (!Hit)
            {
                Hit = true;
                MSM.Instance.LostLife();
                rend.enabled = false;
                alive = false;
                Invoke("ResetPlayer", 1);
            }
        }

        //actions for if the player hits the lever
        if(collision.name.Contains("Lever"))
        {
            LeverScript lever = collision.gameObject.GetComponent<LeverScript>();
            lever.MoveLever();
            lever.MoveDoor();
        }

        //actions for if the player hits the poster
        if (collision.name.Contains("Signpost"))
        {
            string name = collision.name;

            if(name.Contains("One"))
            {
                signNumber = 1;
            }
            else if(name.Contains("Two"))
            {
                signNumber = 2;
            }
            else
            {
                signNumber = 3;
            }

            canvas.EnableText(signNumber);
        }

    }

    //Resets the player 
    public void ResetPlayer()
    {
        MSM.Instance.ResetEverything();
        rbody.position = startPosition;
        rend.enabled = true;
        alive = true;
        cooldown = 0;

        if(upsideDown)
        {
            currentScale = trans.localScale;
            currentScale.y *= -1;
            rbody.transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
        }
        upsideDown = false;
        rbody.gravityScale = 4;
        Hit = false;
    }

    //Changes the x direction the player faces
    private void FlipX()
    {
        facingRight = !facingRight;
        currentScale = rbody.transform.localScale;
        currentScale.x *= -1;
        rbody.transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
    }

    //Flips the y direction of the player
    private void GravityFlip()
    {
        rbody.gravityScale *= -1;
        upsideDown = !upsideDown;

        currentScale = trans.localScale;
        currentScale.y *= -1;
        rbody.transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
        shootDown *= -1;
        Gravity = (!Gravity); 
    }

    //Spawns the player's bullets in the direction the player shoots
    private void SpawnPlayerBullets()
    {
        GameObject bullet = bulletPool.GetObject();
        if (!bullet)
        {
            return;
        }
        if (facingRight)
        {
            
            bullet.transform.position = rbody.position + shootRight + shootDown;
        }
        else
        {
            
            bullet.transform.position = rbody.position - shootRight + shootDown;
        }
        
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        bulletScript.faceRight = facingRight;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Rigidbody2D rbody;
    SpriteRenderer brend;
    public bool faceRight;
    public const float BULLET_SPEED = 20;
    Vector3 currentScale;
    Vector2 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        brend = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Determines which direction the bullet should be facing and moving
        if (faceRight)
        {
            rbody.velocity = new Vector2(BULLET_SPEED, 0);
            currentScale = rbody.transform.localScale;
            currentScale.x *= -1;
            rbody.transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
        }
        else
        {
            rbody.velocity = new Vector2((BULLET_SPEED) * -1, 0);
            currentScale = rbody.transform.localScale;
            currentScale.x *= -1;
            rbody.transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        //If the enemy bullet collides with the player then it is destroyed
        if(collision.name.Equals("Player") && this.name.Contains("EnemyBullet"))
        {
            gameObject.SetActive(false);
            
        }
        //If the player bullet collides with the boss then it is destroyed
        if (collision.name.Equals("boss") && this.name.Contains("PlayerBullet"))
        {
            gameObject.SetActive(false);
        }
        //if the player bullet collides with an enemy then it is destroyed
        else if(collision.name.Contains("Enemy") && this.name.Equals("PlayerBullet") &&
            !collision.name.Contains("EnemyBullet"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}

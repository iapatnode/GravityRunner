using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    Rigidbody2D rbody;
    SpriteRenderer erend;
    Animator ani;
    Vector3 currentScale;

    float startPosition;
    const float SPEED = 5f;
    public BulletScript bulletPrototype;
    System.Random rdm;
    float bulletTime = 0.0f;
    private bool enemyRight;
    float rayDistance;
    public LayerMask player;
    public LayerMask ground;
    bool visible;
    bool ready;
    bool firstSight;
    bool movingRight;
    bool firstPoint;
    objectPool bulletPool;
    Vector2 lastSpeed;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        rdm = new System.Random();
        rbody = GetComponent<Rigidbody2D>();
        erend = GetComponent<SpriteRenderer>();
        startPosition = transform.position.x;
        enemyRight = false; //enemy initially faces to the left
        movingRight = false;
        rayDistance = 20f;
        ready = true;
        bulletPool = new objectPool(bulletPrototype.gameObject, true, 15);
        firstSight = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (visible && !CameraManager.zoomed)
        {
            lastSpeed = rbody.velocity;
            rbody.velocity = Vector2.zero;
            firstSight = true;
            ani.SetBool("Moving", false);
        }

        //Changes the direction of the enemy faces based on the direction he is moving
        //Changes the direction of the enemy faces based on the direction he is moving
        if (rbody.velocity.x > 0 && !enemyRight)
        {
            FlipEnemyDirection();
        }
        else if (rbody.velocity.x < 0 && enemyRight)
        {
            FlipEnemyDirection();
        }


        //Determines when to spawn bullets
        bulletTime -= Time.deltaTime;
        if (/*bulletTime <= 0 &&*/ lineOfSight() && ready)
        {
            ready = false;
            Invoke("SpawnEnemyBullets", 2);
            //SpawnEnemyBullets();
            //bulletTime = 1.5f;
        }
    }

    private void FixedUpdate()
    {

        //Moves the enemy back and forth over a certain distance
        if (transform.position.x >= startPosition + 5 && visible && CameraManager.zoomed || hitWallR() && visible && CameraManager.zoomed)
        {
            print("moving");
            rbody.velocity = new Vector2(-SPEED, 0);
            FlipEnemyDirection();
            ani.SetBool("Moving", true);
        }
        else if (transform.position.x <= startPosition - 5 && visible && CameraManager.zoomed || hitWallL() && visible && CameraManager.zoomed)
        {
            print("moving");
            rbody.velocity = new Vector2(SPEED, 0);
            FlipEnemyDirection();
            ani.SetBool("Moving", true);
        }
    }

    //Spawns bullets in the direction the enemy shoots
    private void SpawnEnemyBullets()
    {
        GameObject enemyBullet = bulletPool.GetObject();
        if (!enemyBullet)
        {
            return;
        }
        enemyBullet.transform.position = rbody.position;
        BulletScript enBulletScript = enemyBullet.GetComponent<BulletScript>();
        enBulletScript.faceRight = enemyRight;
        ready = true;

    }

    //Changes the direction the enemy faces
    private void FlipEnemyDirection()
    {
        enemyRight = !enemyRight;
        currentScale = rbody.transform.localScale;
        currentScale.x *= -1;
        rbody.transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
    }

    //Destroys enemy when hit with player's bullet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("PlayerBullet"))
        {
            //Gives the player points for killing an enemy
            MSM.Instance.IncrementScore();
            Destroy(gameObject);
        }
    }

    bool lineOfSight()
    {
        if (enemyRight)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, player);
            return (hit.collider != null);
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, rayDistance, player);
            return (hit.collider != null);
        }

    }

    bool hitWallR()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(0, .5f, 0), Vector2.right, 4, ground);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(0, -.5f, 0), Vector2.right, 4, ground);
        return (hit1.collider != null) || (hit2.collider != null);
    }

    bool hitWallL()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(0, .5f, 0), Vector2.left, 4, ground);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(0, -.5f, 0), Vector2.left, 4, ground);
        return (hit1.collider != null) || (hit2.collider != null);
    }

    private void OnBecameVisible()
    {
        if (firstSight && CameraManager.zoomed)
        {
            rbody.velocity = new Vector2(-SPEED, 0);
            firstSight = false;
        }
        print("first sight " + firstSight);
        visible = true;
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }
}

using UnityEngine;
using System.Collections;

public class LDEnemyBehaviour : LDObject {

    public int forestspeed = 5;
    public int forestpatrolSpeed = 2;
    public int spacespeed = 5;
    public int spacepatrolSpeed = 2;
    public int cthuluspeed = 5;
    public int cthulupatrolSpeed = 2;
    private int speed;
    private int patrolSpeed;
    public enum EnemyState
    {
        searching, 
        following
    }

    public float timeToChangeDirection = 5.0f;

    public int damage = 40;
    public float damageFrequency = 1.0f;

    private Vector3 lastPosition;

    private float damageTimer;
    private float internalTimer;

    public Vector2 heading;

    private EnemyState currentState;
    private GameObject target;

    void Awake()
    {
        createNewHeading();
        dreamManager = GameObject.FindGameObjectWithTag("DreamManager").GetComponent<LDDreamManager>();
        currentDreamState = LDDreamManager.DreamState.forest;
        damageTimer = damageFrequency;
        currentState = EnemyState.searching;
    }

	// Use this for initialization
    void Start()
    {
        lastCheck = Time.time;
    }

    void Move(Vector2 dir)
    {
        if (currentDreamState == LDDreamManager.DreamState.cthulu)
        {
            speed = cthuluspeed;
            patrolSpeed = cthulupatrolSpeed;
        }

        if (currentDreamState == LDDreamManager.DreamState.forest)
        {
            speed = forestspeed;
            patrolSpeed = forestpatrolSpeed;
        }

        if (currentDreamState == LDDreamManager.DreamState.space)
        {
            speed = spacespeed;
            patrolSpeed = spacepatrolSpeed;
        }

        if (currentState == EnemyState.searching)
        {
            this.rigidbody2D.velocity = dir * patrolSpeed;
        }
        else
        {
            this.rigidbody2D.velocity = dir * speed;
        }
    }

    void FixedUpdate()
    {
        damageTimer += Time.fixedDeltaTime;
        if (currentState == EnemyState.searching)
        {
            Search();
        }
        else
        {
            Follow();
        }
        lastPosition = transform.position;
    }

    private void Follow()
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();
        heading = direction;
        Move(heading);
    }

    private void Search()
    {
        internalTimer += Time.deltaTime;

        if (internalTimer > timeToChangeDirection || lastPosition == transform.position)
        {
            createNewHeading();
            internalTimer = 0.0f;
        }

        Move(heading);
    }

    private void createNewHeading()
    {
        float xCoord = Random.Range(0, 100);

        if (xCoord <= 25) heading = Vector2.right;
        else if (xCoord <= 50) heading = -Vector2.right; 
        else if (xCoord <= 75) heading = Vector2.up; 
        else if (xCoord <= 100) heading = -Vector2.up;

        heading.Normalize();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && damageTimer > damageFrequency)
        {
            Debug.Log("atakujem kurwa");
            other.gameObject.GetComponent<PlayerScript>().Damage(damage);
            damageTimer = 0.0f;
        }

        this.createNewHeading();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerArea")
        {
            currentState = EnemyState.following;
            target = other.gameObject.GetComponent<LDCameraFollow>().player;
        }
        else if (other.gameObject.tag == "Projectile")
        {
            Debug.Log("trafiony");
            dealDamage(other.gameObject.GetComponent<LDProjectile>().damage);
            Destroy(other.gameObject);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerArea")
        {
            currentState = EnemyState.searching;
        }
    }
}

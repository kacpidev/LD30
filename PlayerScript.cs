using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    private float h, v;
    public int health = 100;
    public float speed = 10.0f;
    public float targetspeed;
    public Vector2 currentVelocity;
    public LDInventory inventory = new LDInventory();
    public GameObject camera;
    public bool hasKey;
    public int potionCount = 0;

	// Use this for initialization
	void Start () {
        GameObject c = Instantiate(camera) as GameObject;
        c.GetComponent<LDCameraFollow>().player = this.gameObject;

	}
	
	// Update is called once per frame
    void Update()
    {

         if (Input.GetKeyDown("space"))
        {
            if (potionCount > 0)
            {
                inventory.Use();
                potionCount--;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (inventory.activeWeapon >= 0)
            {
                inventory.weapon.AttackLeft(transform.position + Vector3.left/2);
                }
            }

        if (Input.GetKey(KeyCode.D))
        {
            if (inventory.activeWeapon >= 0)
            {
                inventory.weapon.AttackRight(transform.position + Vector3.right / 2);
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (inventory.activeWeapon >= 0)
            {
                inventory.weapon.AttackUp(transform.position + Vector3.up / 2);
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (inventory.activeWeapon >= 0)
            {
                inventory.weapon.AttackDown(transform.position + Vector3.down / 2);
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        velocity.x = h;
        velocity.y = v;

        velocity.Normalize();

        velocity *= speed;

        rigidbody2D.velocity = velocity;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Debug.Log("player trafiony");
            Damage(other.gameObject.GetComponent<LDProjectile>().damage);
            other.gameObject.GetComponent<LDProjectile>().Die();
        }
    }

    public void Damage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Application.LoadLevel("menu");
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 20), "Health: " + health);
        GUI.Box(new Rect(Screen.width - 110, 10, 100, 20), "Potions: " + potionCount);
    }
}

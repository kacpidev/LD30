using UnityEngine;
using System.Collections;

public class LDObject : LDDreamObject {

    public int health;

	// Use this for initialization
	void Start () {
	
	}

    public void dealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Debug.Log("trafiony");
            dealDamage(other.gameObject.GetComponent<LDProjectile>().damage);
            Destroy(other.gameObject);
        }
    }
}

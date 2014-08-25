using UnityEngine;
using System.Collections;

public class LDWeapon : LDItem {

    public float timeout;
    private float lastAttack;
    public GameObject projectile;
    private GameObject player;
    float damage;

    public LDWeapon(float t, float damage)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastAttack = Time.time;
        timeout = t;
        this.damage = damage;
    }

    public void AttackLeft(Vector3 position){
        if (lastAttack + timeout <= Time.time)
        {
            MonoBehaviour.Instantiate(projectile, position, Quaternion.Euler(new Vector3(0,0,180)));
            lastAttack = Time.time;
        }
    }

    public void AttackRight(Vector3 position)
    {
        if (lastAttack + timeout <= Time.time)
        {
            MonoBehaviour.Instantiate(projectile, position, Quaternion.Euler(new Vector3(0, 0, 0)));
            lastAttack = Time.time;
        }
    }

    public void AttackUp(Vector3 position)
    {
        if (lastAttack + timeout <= Time.time)
        {
            MonoBehaviour.Instantiate(projectile, position, Quaternion.Euler(new Vector3(0, 0, 90)));
            lastAttack = Time.time;
        }
    }
    public void AttackDown(Vector3 position)
    {
        if (lastAttack + timeout <= Time.time)
        {
            MonoBehaviour.Instantiate(projectile, position, Quaternion.Euler(new Vector3(0, 0, 270)));
            lastAttack = Time.time;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerScript>().inventory.setWeapon((LDWeapon)this);
            Destroy(this.gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

public class LDProjectile : MonoBehaviour
{

    public int damage = 10;
    public float speed = 100;
    public float lifetime = 2.0f;
    private Vector3 v;
    private float startTime;
    public GameObject owner;
    // Use this for initialization
    void Start()
    {
        v = new Vector3(speed, 0, 0);
        v = transform.rotation * v;
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this.rigidbody2D.rotation = this.transform.rotation.z;
        this.rigidbody2D.velocity = v;

        if (startTime + lifetime <= Time.time)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }


}

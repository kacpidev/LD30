using UnityEngine;
using System.Collections;

public class LDTurret : LDObject {

    public int damage = 40;
    public float damageFrequency = 1.0f;
    private GameObject target;
    private float damageTimer;
    private TurretState currentState;
    public GameObject bullet;
    public float rotationSpeed=2.0f;
    public Transform spawnPoint;
    public enum TurretState
    {
        search,
        fire
    }

    void Awake()
    {
        dreamManager = GameObject.FindGameObjectWithTag("DreamManager").GetComponent<LDDreamManager>();
        currentDreamState = LDDreamManager.DreamState.forest;
        damageTimer = damageFrequency;
        currentState = TurretState.search;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    damageTimer += Time.fixedDeltaTime;
        if (currentState == TurretState.fire)
        {
            LookAtTarget();
        }
	}

    void LookAtTarget()
    {
        Vector3 tempRotation = Vector3.zero;
        Vector3 vec = (target.transform.position - transform.position);
        tempRotation.z = Mathf.Lerp(target.transform.rotation.eulerAngles.z , Mathf.Atan2(vec.y, vec.x)*Mathf.Rad2Deg - 90, Time.time * rotationSpeed);
        transform.rotation = Quaternion.Euler(tempRotation);

        if (damageTimer > damageFrequency)
        {
            GameObject b = Instantiate(bullet, spawnPoint.position, Quaternion.identity) as GameObject;
            b.transform.rotation = transform.rotation;
            b.transform.Rotate(new Vector3(0,0,90));
            damageTimer = 0.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerArea")
        {
            currentState = TurretState.fire;
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
            currentState = TurretState.search;
        }
    }
}

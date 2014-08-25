using UnityEngine;
using System.Collections;

public class LDPotion : LDItem {

    public int health = 25;

	// Use this for initialization
    override public void Use()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().health += health;

    }
	// Update is called once per frame
	void Update () {
	
	}
}

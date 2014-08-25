using UnityEngine;
using System.Collections;

public class LDCthulu : MonoBehaviour {

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
            Debug.Log("player zbiera");
			if (other.gameObject.GetComponent<PlayerScript>().inventory.AddCthuluPart((LDCthulu)this))
			{
				Destroy(this.gameObject);
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class LDItem : MonoBehaviour {

    public virtual void Use()
    {
        Debug.Log("An item has been used");
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("jest player");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerScript>().inventory.AddPotion();
            Destroy(gameObject);
        }
    }

}

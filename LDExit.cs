using UnityEngine;
using System.Collections;

public class LDExit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject o = other.gameObject;
            
            Debug.Log("player se wychodzi"); 
            if (other.gameObject.GetComponent<PlayerScript>().inventory.cthuluParts.Count == 10)
            {
                //Debug.Log("przeszłeś");
                PlayerPrefs.SetInt("progress", 6);
                Application.LoadLevel("endscreen");
                //Destroy(other.gameObject);
            }
            
        }
    }

}

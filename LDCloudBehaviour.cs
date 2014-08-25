using UnityEngine;
using System.Collections;

public class LDCloudBehaviour : MonoBehaviour {

    public int width;
    public float speed;
    public Vector3 startPosition;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x < width)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            this.transform.position = startPosition;

        }


	}
}

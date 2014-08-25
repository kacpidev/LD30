using UnityEngine;
using System.Collections;

public class LDCameraFollow : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position + Vector3.back*10;
	}
}

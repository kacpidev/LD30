using UnityEngine;
using System.Collections;

public class LDDreamManager : MonoBehaviour {

    public enum DreamState
    {
        forest,
        cthulu,
        space
    }

    public DreamState currentState;
    public float timestamp = 30.0f;
    public float lastTime;

	// Use this for initialization
	void Start () {
        lastTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= lastTime + timestamp)
        {
            int r = Random.Range(0,100);

            if (r < 33) currentState = DreamState.cthulu;
            else if (r < 66) currentState = DreamState.forest;
            else currentState = DreamState.space;
			Debug.Log("skaczemy na " + currentState);
            lastTime = Time.time;
        }
	}
}

using UnityEngine;
using System.Collections;

public class LDTerrain : LDDreamObject {

	private float checkTime;

	void Start () {
		lastCheck = Time.time;
	}
	
	void Awake(){
		dreamManager = GameObject.FindGameObjectWithTag("DreamManager").GetComponent<LDDreamManager>();
		currentDreamState = LDDreamManager.DreamState.forest;
	}
	
	// Update is called once per frame
	void Update () {
		CheckState();
	}

	public void CheckState(){
		if (checkInterval + lastCheck <= Time.time)
		{
			if (currentDreamState != dreamManager.currentState)
			{
				if(checkTime<lastCheck) checkTime = Time.time;
				float r = Random.Range(0, 10000)/10000.0f;
				if (r < Mathf.Pow((Time.time - checkTime)/30.0f, 4))
				{
				//	Debug.Log ("leci zmiana");
					currentDreamState = dreamManager.currentState;
					lastCheck = Time.time;

				}
			}
		}
	}
}

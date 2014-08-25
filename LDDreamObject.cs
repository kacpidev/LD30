using UnityEngine;
using System.Collections;

public class LDDreamObject : MonoBehaviour {

	public Sprite forestSprite;
	public Sprite cthuluSprite;
	public Sprite spaceSprite;

    public LDDreamManager.DreamState currentDreamState;

    public float checkInterval = 10.0f;
    public float lastCheck, checkTime;
    public LDDreamManager dreamManager;

	// Use this for initialization
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
		if (checkInterval + lastCheck >= Time.time)
		{
			//Debug.Log(currentDreamState);
			if (currentDreamState != dreamManager.currentState)
			{
			//	Debug.Log("wlazlem");
				float r = Random.Range(0, 1000)/1000.0f;
				//Debug.Log (Mathf.Log((Time.time - lastCheck)));
				if (r < Mathf.Pow((Time.time - checkTime)/20.0f, 6))
				{
					if(checkTime<lastCheck) checkTime = Time.time;
					currentDreamState = dreamManager.currentState;
					lastCheck = Time.time;
					if (currentDreamState == LDDreamManager.DreamState.space) GetComponent<SpriteRenderer>().sprite = spaceSprite;
					else if (currentDreamState == LDDreamManager.DreamState.forest) GetComponent<SpriteRenderer>().sprite = forestSprite;
					else GetComponent<SpriteRenderer>().sprite = cthuluSprite;
					//Debug.Log ("Zmieniłem stan w " + Time.time + " na " + currentDreamState);
				}
			}
			else
			{
				lastCheck = Time.time;
			}
		}
	}
}
using UnityEngine;
using System.Collections;

public class LDMenu : MonoBehaviour {

    public int progress;

    public GameObject[] slides;


	// Use this for initialization
	void Start () {
        
        if (PlayerPrefs.HasKey("progress"))
        {
            progress = PlayerPrefs.GetInt("progress") + 1; 
            if (progress < 4)
            {
                PlayerPrefs.SetInt("progress", progress);
            }
        }
        else
        {
            progress = 0;
            PlayerPrefs.SetInt("progress", progress);
        }
        PlayerPrefs.Save();
        Instantiate(slides[progress]);
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space)){
            if (progress < 4) Application.LoadLevel("testscene");
            else Application.Quit();
        }
	}
}

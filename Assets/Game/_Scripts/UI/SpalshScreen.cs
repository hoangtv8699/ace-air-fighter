using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpalshScreen : MonoBehaviour
{

 
	void Start ()
	{

		Invoke ("loadMenuLevel", 3.0f);
	 
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	 
	}

	 
	void loadMenuLevel ()
	{
		SceneManager.LoadScene ("MainMenu");
	}

 
	void Update ()
	{

	    
	   
		//to force load on click or touch
		if (Input.GetKey (KeyCode.Mouse0)) {
			loadMenuLevel ();
		}

	 
	}
}

using UnityEngine;
using System.Collections;

public class takeScreenShot : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		//DontDestroyOnLoad (gameObject);
	}

	string resolution;
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.G)) {

		 
			resolution = "" + Screen.width * 8 + "X" + Screen.height * 8;
			ScreenCapture.CaptureScreenshot ("ScreenShot-" + resolution + "-" + PlayerPrefs.GetInt ("number", 0) + ".png", 1);
			PlayerPrefs.SetInt ("number", PlayerPrefs.GetInt ("number", 0) + 1);
			//Debug.Log ("takenShot with " + resolution);

		}
		if (Input.GetKeyDown (KeyCode.H)) {

		 
			resolution = "" + Screen.width * 16 + "X" + Screen.height * 16;
			ScreenCapture.CaptureScreenshot ("ScreenShot-" + resolution + "-" + PlayerPrefs.GetInt ("number", 0) + ".png", 8);
			PlayerPrefs.SetInt ("number", PlayerPrefs.GetInt ("number", 0) + 1);
			//Debug.Log ("takenShot with " + resolution);

		}
	
	}
}

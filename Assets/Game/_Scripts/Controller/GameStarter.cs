using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		if (PlayerPrefs.GetInt ("TutorialShownCount", 0) < 3) {

			PlayerPrefs.SetInt ("TutorialShownCount", PlayerPrefs.GetInt ("TutorialShownCount", 0) + 1);
		} else {

			startGame ();
		}
	}

	public void startGame ()
	{
		GameController.Static.OnGameStart ();
		gameObject.SetActive (false);
	}
}

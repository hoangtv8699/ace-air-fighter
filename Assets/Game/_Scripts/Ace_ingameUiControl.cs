using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum inGameMenuStates
{
	hud,
	resume,
	gameEnd}
;

public class Ace_ingameUiControl : MonoBehaviour
{


	public GameObject hudMenu, resumeMenu, gameEndMenu;
	public Text playerScoreCountText, HealthCountText, CurencyCountText;
	public Image healthBarImage;
	public inGameMenuStates currentMenuState;
	
	public static Ace_ingameUiControl Static;
	public int playerScore;
	public int CoinScore;
	public float startingHealth;

	void OnEnable ()
	{
		Static = this;
		AceButton.buttonDown += OnbuttonClick;
	}

	void Start ()
	{
		ChangeMenuState ();
	 



	}

	public void AddCurency (int Curency)
	{
		CoinScore += Curency;
		CurencyCountText.text = "" + CoinScore;

	}

	public void AddScore (int score)
	{
		playerScore += score;

		playerScoreCountText.text = "" + playerScore;
		AddCurency (1);
	}

	float health;

	public	void UpdateHealthProgress (float healthValue)
	{
		HealthCountText.text = "" + Mathf.RoundToInt (healthValue);
		 
		health = healthValue.Remap (0, startingHealth, 0, 100);
		healthBarImage.fillAmount = Mathf.RoundToInt (health) * 0.01f;
	
		 
	}


	float lastClickTime;

	public	void OnbuttonClick (System.Object buttonname, EventArgs args)
	{
		 
		Debug.Log ("clicked button " + buttonname.ToString ());
		switch (buttonname.ToString ()) {
		case "Pause":
			Debug.Log ("PAUSED");
			currentMenuState = inGameMenuStates.resume;
			SoundController.Static.bgSound.gameObject.SetActive (false);
			SoundController.Static.firingBullets.enabled = false;
			SoundController.Static.PropellerEngine.Stop ();
			Time.timeScale = 0;
			ChangeMenuState ();
			break;
		case "Resume":
			SoundController.Static.firingBullets.enabled = true;
			SoundController.Static.bgSound.gameObject.SetActive (true);
			SoundController.Static.PropellerEngine.Play ();
			currentMenuState = inGameMenuStates.hud;
			Time.timeScale = 1;
			ChangeMenuState ();
			break;

		case "Restart":

			currentMenuState = inGameMenuStates.gameEnd;
			break;

		case "PlayAgain":

			SceneManager.LoadScene (2);
			 
			break;

		case "Mainmenu":
			SceneManager.LoadScene (0);
			break;
			
		}


	}

	public GameEnd gameEndInstance;

	public void ChangeMenuState ()
	{
		switch (currentMenuState) {
		case inGameMenuStates.hud:
			hudMenu.SetActive (true);
			resumeMenu.SetActive (false);
			gameEndMenu.SetActive (false);

			break;
		case inGameMenuStates.resume:

			if (resumeMenu != null) {
				resumeMenu.SetActive (true);
			}
			if (hudMenu != null) {
				hudMenu.SetActive (false);
			}
			break;
		case inGameMenuStates.gameEnd:

			 
			gameEndMenu.SetActive (true);
			hudMenu.SetActive (false);
			resumeMenu.SetActive (false);

			break;
		}
	

	}


 
}

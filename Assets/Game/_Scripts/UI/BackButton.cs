using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;



public class BackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	public GameObject MainMenuParent, ExitMenupg;
	
	void Update () {
		
	}
	void OnEnable(){
		
		OnpointerDown.buttonDown += OnbuttonClick;
	}
	void OnDisable ()
	{
		OnpointerDown.buttonDown -= OnbuttonClick;
	}
	public	void OnbuttonClick(System.Object buttonname,EventArgs args)
	{
		
		SoundController.Static.PlayClickSound ();
		Debug.Log (" clicked on " + buttonname.ToString());
		switch (buttonname.ToString()) {
			//for play button
		case "Back":
			
			//PlayerGroup.SetActive (true);
			//for click sound
			ExitMenupg.SetActive (false);
			MainMenuParent.SetActive (true);
			
			SoundController.Static.PlayClickSound ();
			//MainMenuScreens.currentScreen = MainMenuScreens.MenuScreens.playerSelectionMenu;//for state chenge in to mainmenu screen
			break;
		}
	}
}

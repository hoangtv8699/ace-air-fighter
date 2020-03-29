using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UpgradeSelection : MonoBehaviour {

	public GameObject MainMenuParent, UpgradeSelectionMenu, ExitButton;

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
		case "Upgrades":
			
			//PlayerGroup.SetActive (true);
			//for click sound
			UpgradeSelectionMenu.SetActive (true);
			MainMenuParent.SetActive (false);

			SoundController.Static.PlayClickSound ();
			//MainMenuScreens.currentScreen = MainMenuScreens.MenuScreens.playerSelectionMenu;//for state chenge in to mainmenu screen
			break;
}
	}
}
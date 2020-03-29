using UnityEngine;
using System.Collections;
using System;

public class UnsufficentCoinsForPlayerselection : MonoBehaviour {

	public GameObject UnsufficentCoinsForPlayerselectionMenu, PlayerGroupMenu, PlayerSelectionMenu;
	void Start () {
	
	}
	
	// Update is called once per frame
	
	void OnEnable(){
		
		OnpointerDown.buttonDown += OnbuttonClick;
	}
	void OnDisable ()
	{
		OnpointerDown.buttonDown -= OnbuttonClick;
	}
	//for button control
	public	void OnbuttonClick(System.Object buttonname,EventArgs args)
	{
		Debug.Log (" clicked on " + buttonname.ToString());
		switch (buttonname.ToString()){
			//for ok button
		case "Ok":
			SoundController.Static.PlayClickSound();//for click sound
			UnsufficentCoinsForPlayerselectionMenu.SetActive(false);//for unsufficent menu Disables
			PlayerGroupMenu.SetActive(true);
			PlayerSelectionMenu.SetActive(true);
			//InAppMenuParent.SetActive(true);//inapp menu Enables
		//	MainMenuScreens.currentScreen=MainMenuScreens.MenuScreens;//for moving inapp menu state
			break;
		}
	}
}

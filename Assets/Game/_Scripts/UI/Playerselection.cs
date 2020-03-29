using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public class Playerselection : MonoBehaviour
{

	// Use this for initialization
	public GameObject buyButton, playButton;
	public GameObject  buyPopUp, PlayerSelectionMenu, MainMenuParent, UnsufficentCoinsForPlayerselection, PlayerGroup;
	//public static string levelName;
	void Start ()
	{
	
	}
	
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
		switch (buttonname.ToString()) {
		//for select button
		case "Play":
			//levelName = "NinjaGameplay";
						SoundController.Static.PlayClickSound ();//for click sound
			PlayerSelectionMenu.SetActive (true);
			PlayerGroup.SetActive (true);
           

			break;
		case "playGame":
			//levelName = "NinjaGameplay";
			SoundController.Static.PlayClickSound ();//for click sound
			Application.LoadLevel(1);
			
			
			break;
		//for previous button
		case "Previous":
						SoundController.Static.PlayClickSound ();//for click sound
			showPreviousPlayer ();//for previos polayer information
			break;
		//for next button
		case "Next":
						SoundController.Static.PlayClickSound ();//for click sound
			showNextPlayer ();//for next polayer information
			break;
		//for buy button
		case "Buy":
						SoundController.Static.PlayClickSound ();//for click sound
			PlayerGroup.SetActive (false);//for player disable
			purchasePlayer ();//for player purchase information
						//MainMenuScreens.currentScreen = MainMenuScreens.MenuScreens.UnSufficentCoinsMenu;//for moving unsufficent menu state
			break;
		//for back button
		case "Back":
						SoundController.Static.PlayClickSound ();//for click sound
			PlayerSelectionMenu.SetActive (false);
			PlayerGroup.SetActive (false);
			MainMenuParent.SetActive (true);//for mainmenu enable
			break;
		}
	}
	//for game play
	void LategamePlay ()
	{
		//LoadingRotation.gameplay ();
	}

	public static int PlayerIndex = 0;
	public GameObject[] PlayerMeshObjs;
	public Text PlayerNameText, PlayerPriceDisplayText, headingText;
	//for next player information
	void showNextPlayer ()
	{
		PlayerIndex++;
		if (PlayerIndex > PlayerMeshObjs.Length - 1)
			PlayerIndex = 0;
		for (int PlayerCount=0; PlayerCount<= PlayerMeshObjs.Length-1; PlayerCount ++) {
			PlayerMeshObjs [PlayerCount].SetActive (false);
			
		}
		PlayerMeshObjs [PlayerIndex].SetActive (true);
		showPlayerINFO ();
	}
	//for previous player information
	void showPreviousPlayer ()
	{
		PlayerIndex--;
		if (PlayerIndex < 0)
			PlayerIndex = PlayerMeshObjs.Length - 1;
		for (int PlayerCount=0; PlayerCount<= PlayerMeshObjs.Length-1; PlayerCount ++) {
			PlayerMeshObjs [PlayerCount].SetActive (false);
			
		}
		PlayerMeshObjs [PlayerIndex].SetActive (true);
		showPlayerINFO ();
	}
	//for showing player information
	void showPlayerINFO ()
	{
		
		switch (PlayerIndex) {
		//for player 1 information 
		case 0:
			headingText.text = "Select Player ";
			PlayerNameText.text = "Boeing F4B ";
			PlayerPriceDisplayText.text = "FREE ";
			playButton.SetActive (true);
			buyButton.SetActive (false);
			break;
		//for player 2 information 
		case 1:
			headingText.text = "Select Player ";
			PlayerNameText.text = "Martin PBM-3";
			PlayerPriceDisplayText.text = "1000 ";
			if (PlayerPrefs.GetInt ("isPlayer1Purchased", 0) == 1) {
				PlayerPriceDisplayText.text = "UnLocked ";
				playButton.SetActive (true);
				buyButton.SetActive (false);
			} else {
				 
				buyButton.SetActive (true);
				playButton.SetActive (false);
			}
			break;
		//for player 3 information 
		case 2:
			headingText.text = "Select Player ";
			PlayerNameText.text = "Brewster XA-32";
			PlayerPriceDisplayText.text = "3000 ";
			
			if (PlayerPrefs.GetInt ("isPlayer2Purchased", 0) == 1) {
				PlayerPriceDisplayText.text = "UnLocked ";
				playButton.SetActive (true);
				buyButton.SetActive (false);
			} else {
				 
				buyButton.SetActive (true);
				playButton.SetActive (false);
			}
			break;
		//for player 4 information 
		case 3:
			headingText.text = "Select Player ";
			PlayerNameText.text = "Bell XP-77";
			PlayerPriceDisplayText.text = "4000 ";
			
			if (PlayerPrefs.GetInt ("isPlayer3Purchased", 0) == 1) {
				PlayerPriceDisplayText.text = "UnLocked ";
				playButton.SetActive (true);
				buyButton.SetActive (false);
			} else {

				buyButton.SetActive (true);
				playButton.SetActive (false);
			}
			break;
		//for player 5 information 
		case 4:
			headingText.text = "Select Player ";
			PlayerNameText.text = "Curtiss P-36 Hawk";
			PlayerPriceDisplayText.text = "5000 ";
			
			if (PlayerPrefs.GetInt ("isPlayer4Purchased", 0) == 1) {
				PlayerPriceDisplayText.text = "UnLocked ";
				playButton.SetActive (true);
				buyButton.SetActive (false);
			} else {
				 
				buyButton.SetActive (true);
				playButton.SetActive (false);
			}
			break;
		
		//for player 6 information 
		case 5:
			headingText.text = "Select Player ";
			PlayerNameText.text = "Name : FFF";
			PlayerPriceDisplayText.text = "7000 ";
			
			if (PlayerPrefs.GetInt ("isPlayer5Purchased", 0) == 1) {
				PlayerPriceDisplayText.text = "UnLocked ";
				playButton.SetActive (true);
				buyButton.SetActive (false);
			} else {

				buyButton.SetActive (true);
				playButton.SetActive (false);
			}
			break;
		}
		
	}
	//for  player purchase information
	void purchasePlayer ()
	{
		
		switch (PlayerIndex) {
		//for player 2
		case 1:
			
			if (TotalCoins.Static.totalCoins >= 1000) {
				buyPopUP.PlayerCost = 1000;//to set the cost in buyPopUpScript
				buyPopUp.SetActive (true);
				PlayerSelectionMenu.SetActive (false);
			} else {
				///	InAPPMenu.SetActive(true);
				//	gameObject.SetActive(false);
				UnsufficentCoinsForPlayerselection.SetActive (true);
				PlayerSelectionMenu.SetActive (false);
			}
			
			break;
		//for player 3
		case 2:

			if (TotalCoins.Static.totalCoins >= 3000) {
				buyPopUP.PlayerCost = 3000;
				buyPopUp.SetActive (true);
				PlayerSelectionMenu.SetActive (false);
			} else {
				//	InAPPMenu.SetActive(true);
				//PlayerSelectionMenu.SetActive(false);
				UnsufficentCoinsForPlayerselection.SetActive (true);
				PlayerSelectionMenu.SetActive (false);
			}
			
			break;
		//for player 4
		case 3:
			if (TotalCoins.Static.totalCoins >= 4000) {
				buyPopUP.PlayerCost = 4000;
				buyPopUp.SetActive (true);
				PlayerSelectionMenu.SetActive (false);
			} else {
				//InAPPMenu.SetActive(true);
				//PlayerSelectionMenu.SetActive(false);
				UnsufficentCoinsForPlayerselection.SetActive (true);
				PlayerSelectionMenu.SetActive (false);
			}
			
			break;
		//for player 5
		case 4:
			if (TotalCoins.Static.totalCoins >= 5000) {
				buyPopUP.PlayerCost = 5000;
				buyPopUp.SetActive (true);
				PlayerSelectionMenu.SetActive (false);
			} else {
				//	InAPPMenu.SetActive(true);
				//PlayerSelectionMenu.SetActive(false);
				UnsufficentCoinsForPlayerselection.SetActive (true);
				PlayerSelectionMenu.SetActive (false);
			}
			
			break;
		//for player 6
		case 5:
			if (TotalCoins.Static.totalCoins >= 6000) {
				buyPopUP.PlayerCost = 6000;
				buyPopUp.SetActive (true);
				PlayerSelectionMenu.SetActive (false);

			} else {
				//	InAPPMenu.SetActive(true);
				//	PlayerSelectionMenu.SetActive(false);
				UnsufficentCoinsForPlayerselection.SetActive (true);
				PlayerSelectionMenu.SetActive (false);
			}
			
			break;
		}
		
	}
}

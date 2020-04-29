using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public enum scorecardstates
{

	show_score,
	show_bestScore,
	show_buttons,
	show_currency,
	None}
;

public class GameEnd : MonoBehaviour
{

	public float startScoreCount, startBestScorCount, startEarnedCurrency,
		TargetScoreCount, TargetBestScorCount, targetEarnedCurrency;

	public float toreachScore, toreachBestScore, toReachCurrency;

	public float valueForScore, ValueForBestScore, valueFoeCurrency;
	public Text finalScore, bestScore, ErnedCurrencyText;
	 
	public GameObject buttonGroup;
 
	public GameObject newSticker;
	public scorecardstates presentstate;
	public static EventHandler showAds;

	void Start ()
	{
		 
		//PlayerPrefs.SetInt ("TotalCurrency", PlayerPrefs.GetInt ("TotalCurrency", 0) + Ace_ingameUiControl.Static.CoinScore);

		 

		//FinalScoreCount ();

		//BestScore ();
		//presentstate = scorecardstates.show_score;
		//buttonGroup.SetActive (false);
		//FinalCurrency ();
		//newSticker.SetActive (false);

		//finalScore.text = " ";
		//ErnedCurrencyText.text = " ";
		//bestScore.text = " ";
	}


	//void Update ()
	//{

	//	switch (presentstate) {
		
	//	case scorecardstates.show_score:
	//		valueForScore = Mathf.Lerp (valueForScore, toreachScore, 0.4f);
	//		finalScore.text = "" + Mathf.RoundToInt (valueForScore);
	//		if (Mathf.RoundToInt (valueForScore) == toreachScore) {
	//			presentstate = scorecardstates.show_bestScore;
	//		}
	//		break;
	//	case scorecardstates.show_bestScore:
	//		ValueForBestScore = Mathf.Lerp (ValueForBestScore, toreachBestScore, 0.6f);
	//		bestScore.text = "" + Mathf.RoundToInt (ValueForBestScore) + "  ";
	//		if (Mathf.RoundToInt (ValueForBestScore) == toreachBestScore) {
	//			presentstate = scorecardstates.show_currency;
	//		}
	//		break;
		
	//	case scorecardstates.show_currency:
	//		valueFoeCurrency = Mathf.Lerp (valueFoeCurrency, toReachCurrency, 0.4f);
	//		ErnedCurrencyText.text = "  " + Mathf.RoundToInt (valueFoeCurrency) + "  ";
	//		if (Mathf.RoundToInt (valueFoeCurrency) == toReachCurrency) {
	//			presentstate = scorecardstates.show_buttons;
	//		}
	//		break;



	//	case scorecardstates.show_buttons:
	//		if (isAchivedNewBest)
	//			newSticker.SetActive (true);
	//		buttonGroup.SetActive (true);
	//		if (showAds != null)
	//			showAds (null, null); //you can register to this event ,to show ads
	//		//SoundController.Static.StopBG();
	//		presentstate = scorecardstates.None;
	//		//ErnedCurrencyText.text="Erned Currency     :   " +(int)TargetScoreCount*100;
	//		break;
	//	}
	
	//}

	//void FinalScoreCount ()
	//{
	//	TargetScoreCount = Ace_ingameUiControl.Static.playerScore;
	//	toreachScore = TargetScoreCount;
		 
	
	//}


	//void FinalCurrency ()
	//{
	//	targetEarnedCurrency = Ace_ingameUiControl.Static.CoinScore;
	//	toReachCurrency = targetEarnedCurrency;

	//}

	//void ChangeScoreCount (int newScoreCount)
	//{
	//	finalScore.text = "" + newScoreCount;
	//}

	//bool isAchivedNewBest = false;

	//void BestScore ()
	//{
	//	toreachBestScore = Mathf.RoundToInt (PlayerPrefs.GetInt ("BestScore", 0));
	//	if (PlayerPrefs.GetInt ("BestScore", 0) < Ace_ingameUiControl.Static.playerScore) {

	//		TargetBestScorCount = Ace_ingameUiControl.Static.playerScore;
	//		toreachBestScore = TargetBestScorCount;
	//		isAchivedNewBest = true;
	//		PlayerPrefs.SetInt ("BestScore", Ace_ingameUiControl.Static.playerScore);
	//	}
	
	//}

	 

}

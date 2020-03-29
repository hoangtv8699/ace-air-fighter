using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class StoreUpgradeHandler : MonoBehaviour {

	public int[] purchaseCurrency;
	public float[] ProgressFillAmount;
	public Image ProgressFill;
	public Text CurrentUpgradeCurrency;
	public string BuyButtonName="BUY";
	public GameObject NoCurrenPopUp;
	public GameObject DeactivateMenu;
	public GameObject OnCompleteDisableObject;
	public Text onCompleteStringText;
	public string stringToWriteOnComplete;
	void OnEnable () {
		AceButton.buttonDown += OnButtonDown;
		ShowProgress ();
	}

	void OnDisable () {
		AceButton.buttonDown -= OnButtonDown;
	}

	void ShowProgress()
	{

		if (PlayerPrefs.GetInt (gameObject.name, 0) == 0) {
			ProgressFill.fillAmount = 0;
		}
		else 	ProgressFill.fillAmount = ProgressFillAmount [PlayerPrefs.GetInt (gameObject.name, 0)];

		if (ProgressFill.fillAmount >= 1) {
			OnCompleteDisableObject.SetActive(false);
			onCompleteStringText.text = stringToWriteOnComplete;
		}


		CurrentUpgradeCurrency.text = "" + purchaseCurrency[ Mathf.Clamp( PlayerPrefs.GetInt (gameObject.name, 0),0,purchaseCurrency.Length-1 )];
	}

	void OnButtonDown(System.Object obj,EventArgs args)
	{

		if (obj.ToString ().Contains ( BuyButtonName)  && ProgressFill.fillAmount !=1 ) {

			if( TotalCurrency.Static.totalCurrency >= purchaseCurrency[ PlayerPrefs.GetInt(gameObject.name,0)] )
			{

				TotalCurrency.Static.SubtractCurrency(purchaseCurrency[PlayerPrefs.GetInt(gameObject.name,0)]); 

				PlayerPrefs.SetInt(  gameObject.name,    PlayerPrefs.GetInt(gameObject.name,0)+1);
				ShowProgress();
			}
			else 
			{		 
				NoCurrenPopUp.SetActive(true);
				DeactivateMenu.SetActive(false);


			}  
		}
	}
	 
}

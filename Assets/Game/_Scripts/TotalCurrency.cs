using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TotalCurrency : MonoBehaviour
{


	 
	public static TotalCurrency Static;
	public  Text TotalCurrencytext;
	public int totalCurrency ;
 

	void Start ()
	{
		UpdateCurrency ();
		Static = this;
		Time.timeScale = 1;
	}
	
	public  void UpdateCurrency ()
	{
		totalCurrency = PlayerPrefs.GetInt ("TotalCurrency", 0);
		TotalCurrencytext.text = "" + PlayerPrefs.GetInt ("TotalCurrency", 0);
	}

	public void AddCurrency (int AddingCurrency)
	{
		PlayerPrefs.SetInt ("TotalCurrency", PlayerPrefs.GetInt ("TotalCurrency", 0) + AddingCurrency);
		UpdateCurrency ();
	}

	public void SubtractCurrency (int SubtractingCurrency)
	{
		PlayerPrefs.SetInt ("TotalCurrency", PlayerPrefs.GetInt ("TotalCurrency", 0) - SubtractingCurrency);
		UpdateCurrency ();
	}

	public static int getCurrencyCount ()
	{
		return PlayerPrefs.GetInt ("TotalCurrency", 0);
	}

	public void ClearCurrency ()
	{
		PlayerPrefs.DeleteAll ();
	}

 
	 

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.P)) {
			AddCurrency (100000);
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			PlayerPrefs.DeleteAll ();
		}
	
	}

	 

}

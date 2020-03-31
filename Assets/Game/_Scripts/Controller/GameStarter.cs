using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}

	public void startGame ()
	{
		GameController.Static.OnGameStart ();
		gameObject.SetActive (false);
	}
}

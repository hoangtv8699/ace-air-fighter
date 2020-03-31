using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public enum ButtonType
{
	
	EventGenerator,
	TargetActiveDeactiveParent,
	TargetActiveOnly,
	TargetDeActiveOnly,
	ScriptActive,
	ScriptDisable,
	SetPlayerPrefence,
	OpenUrl,
	ApplicationQuit,
	LoadNextLevel,
	LoadLevel,
	LoadedLevel,
	GamePaused,
	Gameresumed,
	BroadCastMessage,
	FB,
	openReview}
;

[Serializable]


public class AceButton   : MonoBehaviour ,IPointerDownHandler,IPointerUpHandler
{
	public ButtonType SelectedButtonType;
	public GameObject ObjectToActivate, ObjectToDeActivate;
	public MonoBehaviour behaviour;
	public string stringValue = "PlayerIndex";
	public int playerPrefValueToStore = 0;
	public string levelToLoad;

	
	void Start ()
	{
		//Debug.LogError ("Update URL for Windows");
		
		//validateData ();
	}

	void Update ()
	{
		
	}

	public static EventHandler buttonDown, buttonUp;
	
	// Update is called once per frame
	void onButtonDown (System.Object obj, EventArgs args)
	{
        
		SoundController.Static.PlayClickSound ();
		if (!obj.ToString ().Contains (gameObject.name))
			return;
		
		switch (SelectedButtonType) {
		case ButtonType.EventGenerator:
			break;
			
		case ButtonType.TargetActiveDeactiveParent:
                Debug.Log("gameobject : " + this.name);
                Debug.Log("activate: " + ObjectToActivate.name);
                Debug.Log("de activate: " + ObjectToDeActivate.name);
			ObjectToActivate.SetActive (true);
			ObjectToDeActivate.SetActive (false);
			break;
			
			
		case ButtonType.TargetActiveOnly:
			ObjectToActivate.SetActive (true);
			break;
		case ButtonType.TargetDeActiveOnly:
			ObjectToActivate.SetActive (false);
			break;

			
		case ButtonType.ScriptActive:
			behaviour.enabled = true;
			break;
			
		case ButtonType.ScriptDisable:
			behaviour.enabled = false;
			break;
			
			
		case ButtonType.SetPlayerPrefence:
			PlayerPrefs.SetInt (stringValue, playerPrefValueToStore);
			print ("PlayerPrefs" + PlayerPrefs.GetInt (stringValue, playerPrefValueToStore));
			break;
			
		case ButtonType.OpenUrl:
			OpenUrl ();
			break;
			
		case ButtonType.ApplicationQuit:
			Application.Quit ();
			Debug.Log ("Exit");
			break;
			
		case ButtonType.LoadNextLevel:
			
			
			Application.LoadLevelAsync (Application.loadedLevel + 1);
			
			
			break;
			
		case ButtonType.LoadLevel:
			Application.LoadLevelAsync (levelToLoad);
			
			break;
		case ButtonType.LoadedLevel:
			Application.LoadLevel (Application.loadedLevel);
			break;
		case ButtonType.GamePaused:
			Time.timeScale = 0;
			break;
		case ButtonType.Gameresumed:
			Time.timeScale = 1;
			break;


		case ButtonType.FB:
			Application.OpenURL ("https://www.facebook.com/AceGamesHyderabad/?fref=ts");

			break;
		case ButtonType.openReview:
			Application.OpenURL (stringValue);
			break;
		}
	}

	#region ButtonUP

	void onButtonUp (System.Object obj, EventArgs args)
	{
		switch (SelectedButtonType) {
		case ButtonType.EventGenerator:
			break;
			
		case ButtonType.TargetActiveDeactiveParent:
			
			break;
			
			
		case ButtonType.TargetActiveOnly:
			
			break;
			
		case ButtonType.ScriptActive:
			
			break;
			
		case ButtonType.ScriptDisable:
			
			break;
			
		case ButtonType.SetPlayerPrefence:
			
			break;
			
		case ButtonType.OpenUrl:
			
			break;
			
		case ButtonType.ApplicationQuit:
			
			break;
			
		case ButtonType.LoadNextLevel:
			
			break;
			
		case ButtonType.LoadLevel:
			
			break;
		case ButtonType.GamePaused:
			
			break;
		case ButtonType.Gameresumed:
			
			break;
		}
	}

	#endregion

	
	void OpenUrl ()
	{
		 
		#if UNITY_IPHONE
		Application.OpenURL("https://itunes.apple.com/us/developer/kiran-kumar/id674476457");
		# elif UNITY_ANDROID
		Application.OpenURL ("https://play.google.com/store/apps/developer?id=Ace+Games");
		#elif UNITY_WP8
		Application.OpenURL("https://play.google.com/store/apps/developer?id=Ace+Games");
		#endif
	}
	
	//handling clickEvents
	public void OnPointerDown (PointerEventData data)
	{
		
		if (buttonDown != null)
			buttonDown (gameObject.name, null);
		
	}

	public void OnPointerUp (PointerEventData data)
	{
		
		if (buttonUp != null)
			buttonUp (gameObject.name, null);
	}

	
	void OnEnable ()
	{
		
		buttonDown += onButtonDown;
		buttonUp += onButtonUp;
		
	}

	void OnDisable ()
	{
		
		buttonDown -= onButtonDown;
		buttonUp -= onButtonUp;
		
		
	}

	void validateData ()
	{
		switch (SelectedButtonType) {
		case ButtonType.EventGenerator:
			break;
			
		case ButtonType.TargetActiveOnly:
		case ButtonType.TargetActiveDeactiveParent:
			
			if (ObjectToActivate = null)
				Debug.LogError (gameObject.name + " plz give reference to ObjectToActivate variable");
			
			break;
			
			
		case ButtonType.ScriptActive:
			
		case ButtonType.ScriptDisable:
			if (behaviour = null)
				Debug.LogError (gameObject.name + " plz give reference to behaviour variable");
			break;
			
		case ButtonType.SetPlayerPrefence:
			
			break;
			
		case ButtonType.OpenUrl:
			
			break;
			
		case ButtonType.ApplicationQuit:
			
			break;
			
		case ButtonType.LoadNextLevel:
			
			break;
			
		case ButtonType.LoadLevel:
			
			break;
		case ButtonType.LoadedLevel:
			
			break;
		case ButtonType.GamePaused:
			
			break;
		case ButtonType.Gameresumed:
			
			break;
		}
	}
}

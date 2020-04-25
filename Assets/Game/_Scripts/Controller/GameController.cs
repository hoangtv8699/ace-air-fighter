using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

	// Use this for initialization
	[Range (10, 150)]
	public float
		SingleGunBulletSpeed = 90;
	[Range (10, 150)]
	public float
		DualGunBulletSpeed = 90;
	[Range (10, 150)]
	public float
		TripleGunBulletSpeed = 90;
	[Range (10, 150)]
	public float
		QuadGunBulletSpeed = 90;

	// Use this for initialization
	[Range (20, 150)]
	public float
		PlayerSingleGunBulletSpeed = 90;
	[Range (20, 150)]
	public float
		PlayerDualGunBulletSpeed = 90;
	[Range (20, 150)]
	public float
		PlayerTripleGunBulletSpeed = 90;
	[Range (20, 150)]
	public float
		PlayerQuadGunBulletSpeed = 90;
	GameObject player;
	public static GameController Static;
	public GameObject[] PlayersPrefabs;
	public MonoBehaviour[] ScriptsToEnable, ScriptsToDisable;
	public Color[] enemyBulletColors;
	public GameObject[] ExplosionParticles;
	public GameObject[] SmokeParticles;
	public GameObject[] PlayerParticleEffects;

    private Client client;


	void OnEnable ()
	{
		

		Static = this;
		CreatePlayerOnScene ();
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();

    }

	#if UNITY_EDITOR
	void Update ()
	{
        
        if (!client.IsPlaying())
        {
            Debug.Log("gamecontroller");
            client.CreatePlayerOnScence();
        }
        else
        {
            // synchronized player
        }
        if (Input.GetKey (KeyCode.I)) {
			PlayerPrefs.DeleteAll ();
		}

	}

	#endif

	void CreatePlayerOnScene ()
	{
        
		//player = GameObject.FindGameObjectWithTag ("Player");
		//if (player == null)
		//	player = GameObject.Instantiate (PlayersPrefabs [PlayerPrefs.GetInt ("PlayerIndex", 0)]) as GameObject;
            
          
	}

	public void OnGameStart ()
	{

		foreach (MonoBehaviour script in ScriptsToEnable) {
			script.enabled = true;
		}

	}

	public void OnPlayerDead ()
	{
		SoundController.Static.firingBullets.Stop ();
		SoundController.Static.PropellerEngine.Stop ();
		Invoke ("OnGameEnd", 2);
		
	}

	void OnGameEnd ()
	{

		//SoundController.Static.bgSound.enabled = false;

		//foreach (MonoBehaviour script in ScriptsToDisable) {
		//	script.enabled = false;
		//}


		//Ace_ingameUiControl.Static.currentMenuState = inGameMenuStates.gameEnd;
		//Ace_ingameUiControl.Static.ChangeMenuState ();
	}

   



	 
}

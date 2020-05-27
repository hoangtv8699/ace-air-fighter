using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum inGameMenuStates
{
	hud,
	resume,
	gameEnd
};

public class Ace_ingameUiControl : MonoBehaviour
{


	public GameObject hudMenu, resumeMenu, gameEndMenu, tutorial;
    public Text[] namePlayer;
    public Text[] score;
    public Text[] finalNamePlayer;
    public Text[] finalScore;
    //public Text playerScoreCountText, HealthCountText, CurencyCountText;
    //public Image healthBarImage;
    public inGameMenuStates currentMenuState;
	
	public static Ace_ingameUiControl Static;
    //public int playerScore;
    //public int CoinScore;
    //public float startingHealth;

    private Client client;

	void OnEnable ()
	{
		Static = this;
		//AceButton.buttonDown += OnbuttonClick;
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
    }

	void Start ()
	{
		//ChangeMenuState ();
	}

    private void UpdateScore()
    {
        client.updateScore(namePlayer, score);
    }

    private void FinalScore()
    {
        client.updateScore(finalNamePlayer, finalScore);
    }

    private void Update()
    {
        UpdateScore();
        if (client.checkEndGame())
        {
            gameEndMenu.SetActive(true);
        }
    }


    public void Pause()
    {
        if (!resumeMenu.activeSelf)
        {
            SoundController.Static.bgSound.gameObject.SetActive(false);
            SoundController.Static.firingBullets.enabled = false;
            SoundController.Static.PropellerEngine.Stop();

            if (resumeMenu != null)
            {
                resumeMenu.SetActive(true);
            }
        }
        else
        {
            SoundController.Static.firingBullets.enabled = true;
            SoundController.Static.bgSound.gameObject.SetActive(true);
            SoundController.Static.PropellerEngine.Play();

            if (resumeMenu != null)
            {
                resumeMenu.SetActive(false);
            }
        }

        if (!client.IsPlaying())
        {
            
            if (!tutorial.activeSelf)
            {
                tutorial.SetActive(true);
            }else
            {
                tutorial.SetActive(false);
            }
        }
    }

    public void QuitRoom()
    {
        client.QuitRoom();
    }

    public void MainMenu()
    {
        client.CloseThread();
        SceneManager.LoadScene("MainMenu");
    }

}

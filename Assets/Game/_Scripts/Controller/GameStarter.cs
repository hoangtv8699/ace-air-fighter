using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour
{
    private Client client;

    private void OnEnable()
    {
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
    }

    // Use this for initialization
    void Start ()
	{
		
	}

	public void startGame ()
	{
        client.Ready();
        client.isPlaying = true;
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
        
    }

    private void Update()
    {
        if (client.IsPlaying() && gameObject.activeSelf)
        {
            Debug.Log("start game");
            GameController.Static.OnGameStart();
            gameObject.SetActive(false);
        }
    }
}

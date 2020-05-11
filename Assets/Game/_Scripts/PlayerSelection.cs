using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
[Serializable]
public class players
{
	public GameObject playerObject;
	public int price ;
	public string[] info;
}
public class PlayerSelection : MonoBehaviour
{
	public players[]
		AllPlayers;
	public Transform RotationTransform;
	public Vector3 RotationSpeed ;
	public Text[] infoTexts;
	public static int playerIndex ;
	public Text selectButtonText;
    public GameObject LevelSelection, PlayerMesh, Menu;
    public GameObject selectionMenu;

    private Client client;

    private void Start()
    {
        
    }

    void OnEnable ()
	{
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
        if (client != null)
        {
            Debug.Log("not null");
        }
        playerIndex = 1;
		UpdateInfo ();	
	}

    public void OnBack()
    {
        Debug.Log("Log out");
        client.TcpSend("QUIT|" + client.getString("PlayerName"));
        client.remove("PlayerName");
        Debug.Log("gameobject : " + this.name);
        Debug.Log("activate: " + Menu.name);
        Debug.Log("activate: " + PlayerMesh.name);
        Debug.Log("de activate: " + gameObject.name);
        Menu.SetActive(true);
        PlayerMesh.SetActive(false);
        gameObject.SetActive(false);
    }

	public void OnselectPress ()
	{
        Debug.Log("gameobject : " + this.name);
        Debug.Log("activate: " + LevelSelection.name);
        Debug.Log("de activate: " + PlayerMesh.name);
        Debug.Log("de activate: " + gameObject.name);
        LevelSelection.SetActive(true);
        PlayerMesh.SetActive(false);
        gameObject.SetActive(false);
    }

	void UpdateInfo ()
	{
		limitBoundsForIndex ();

		for (int i=0; i < infoTexts.Length; i++) {
			infoTexts [i].text = AllPlayers [playerIndex - 1].info [i];
		}

        selectButtonText.text = "Select";
        AllPlayers [playerIndex - 1].playerObject.SetActive (true);
		client.setInt ("PlayerIndex", playerIndex);
	}
	public void OnNext ()
	{
		playerIndex++;
		UpdateInfo ();
	}
	
	public void OnPrevious ()
	{
		playerIndex--;
		UpdateInfo ();
	}

	void Update ()
	{
	
		if (RotationTransform != null) {
			RotationTransform.Rotate (RotationSpeed * Time.deltaTime);
		}
	}

	void limitBoundsForIndex ()
	{
		if (playerIndex > AllPlayers.Length) {
			playerIndex = 1;
			
		} else if (playerIndex < 1) {
			playerIndex = AllPlayers.Length;
			
		}

		foreach (players loop in AllPlayers) {
			loop.playerObject.SetActive (false);
		}
	}
}

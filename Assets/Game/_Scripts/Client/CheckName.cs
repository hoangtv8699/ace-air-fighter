using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckName : MonoBehaviour {

    public GameObject PlayerSelection, Menu, PlayerMesh;
    public InputField input;

    private Client client;
    private int numOfLevel;
    
    void OnEnable () {
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
    }

    

    void activeAndDeactive()
    {
        Debug.Log("gameobject : " + this.name);
        Debug.Log("activate: " + PlayerSelection.name);
        Debug.Log("activate: " + PlayerMesh.name);
        Debug.Log("de activate: " + Menu.name);
        PlayerSelection.SetActive(true);
        PlayerMesh.SetActive(true);
        Menu.SetActive(false);
    }
	
    void execute()
    {
        if (client.login(input.text))
        {
            activeAndDeactive();
        }
    }

    

	void Update () {
        if(client.getString("PlayerName") != null)
        {
            activeAndDeactive();
        }
	}
}

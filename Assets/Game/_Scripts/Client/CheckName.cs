using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckName : MonoBehaviour {

    public GameObject PlayerSelection, Menu, PlayerMesh;
    public InputField input;
    public Transform Client;

    private Client client;
    private int numOfLevel;
    
    void OnEnable () {
        GameObject tmp = GameObject.FindGameObjectWithTag("client");
        if(tmp == null)
        {
            Transform temp = GameObject.Instantiate(Client);
            temp.name = "client";
            client = temp.GetComponent<Client>();
        }
        else
        {
            client = tmp.GetComponent<Client>();
        }
    }

    

    void activeAndDeactive()
    {
        //Debug.Log("gameobject : " + this.name);
        //Debug.Log("activate: " + PlayerSelection.name);
        //Debug.Log("activate: " + PlayerMesh.name);
        //Debug.Log("de activate: " + Menu.name);
        PlayerSelection.SetActive(true);
        PlayerMesh.SetActive(true);
        Menu.SetActive(false);
    }
	
    void execute()
    {
        client.login(input.text);
        
    }

    

	void Update () {
        if (client.getString("PlayerName") != null)
        {
            activeAndDeactive();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class RoomSelection : MonoBehaviour {

    public GameObject LevelSelection, loading;
    public Text infoTexts;

    private int RoomIndex;
    private int maxRoomIndex;
    private Client client;


    private void Start()
    {
        

    }

    void OnEnable()
    {
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
        maxRoomIndex = client.getInt("Level" + client.getInt("LevelSelect"));
        RoomIndex = 1;
        UpdateInfo();
    }

    public void OnBack()
    {
        Debug.Log("gameobject : " + this.name);
        Debug.Log("de activate: " + gameObject.name);
        Debug.Log("activate: " + LevelSelection.name);
        gameObject.SetActive(false);
        LevelSelection.SetActive(true);
    }

    public void OnselectPress()
    {
        if (client.joinRoom(RoomIndex))
        {
            client.setInt("RoomSelect", RoomIndex);
            Debug.Log("gameobject : " + this.name);
            Debug.Log("activate: " + loading.name);
            Debug.Log("de activate: " + gameObject.name);
            loading.SetActive(true);
            gameObject.SetActive(false);
            
        }
    }
 
    void UpdateInfo()
    {
        limitBoundsForIndex();
        infoTexts.text = "Room " + RoomIndex;
    }
    public void OnNext()
    {
        RoomIndex++;
        UpdateInfo();
    }

    public void OnPrevious()
    {
        RoomIndex--;
        UpdateInfo();
    }

    void Update()
    {

    }

    void limitBoundsForIndex()
    {
        if (RoomIndex > maxRoomIndex)
        {
            RoomIndex = 1;

        }
        else if (RoomIndex < 1)
        {
            RoomIndex = maxRoomIndex;

        }

    }
}

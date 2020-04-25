using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public class LevelSelection : MonoBehaviour {

    public GameObject RoomSelection, PlayerSelection, PlayerMesh;
    public Text infoTexts;

    private int LevelIndex;
    private int maxLevelIndex;
    private Client client;


    private void Start()
    {
        
    }
    void OnEnable()
    {
        client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
        maxLevelIndex = client.getInt("NumOfLevel");
        LevelIndex = 1;
        UpdateInfo();
    }

    public void OnBack()
    {
        Debug.Log("gameobject : " + this.name);
        Debug.Log("de activate: " + gameObject.name);
        Debug.Log("activate: " + PlayerMesh.name);
        Debug.Log("activate: " + PlayerSelection.name);
        gameObject.SetActive(false);
        PlayerMesh.SetActive(true);
        PlayerSelection.SetActive(true);
    }

    public void OnselectPress()
    {
        client.setInt("LevelSelect", LevelIndex);
        Debug.Log("gameobject : " + this.name);
        Debug.Log("activate: " + RoomSelection.name);
        Debug.Log("de activate: " + gameObject.name);
        RoomSelection.SetActive(true);
        gameObject.SetActive(false);
        
    }

    void UpdateInfo()
    {
        limitBoundsForIndex();
        infoTexts.text = "Level " + LevelIndex;
    }
    public void OnNext()
    {
        LevelIndex++;
        UpdateInfo();
    }

    public void OnPrevious()
    {
        LevelIndex--;
        UpdateInfo();
    }

    void Update()
    {

    }

    void limitBoundsForIndex()
    {
        if (LevelIndex > maxLevelIndex)
        {
            LevelIndex = 1;

        }
        else if (LevelIndex < 1)
        {
            LevelIndex = maxLevelIndex;

        }

    }
}

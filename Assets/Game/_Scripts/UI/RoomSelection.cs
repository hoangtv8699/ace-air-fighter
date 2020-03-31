using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class RoomSelection : MonoBehaviour {

    public Text infoTexts;
    private int RoomIndex;
    public int maxRoomIndex;
    public GameObject NextMenuObject;
    public GameObject selectionMenu;
    void OnEnable()
    {


        RoomIndex = 1;
        UpdateInfo();

        AceButton.buttonDown += OnButtonClick;

        selectionMenu.SetActive(true);

    }

    void OnDisable()
    {
        AceButton.buttonDown -= OnButtonClick;
    }
    void OnButtonClick(System.Object obj, EventArgs args)
    {

        switch (obj.ToString())
        {
            case "Select":

                OnselectPress();
                break;

            case "Next":
                OnNext();
                break;
            case "Previous":
                OnPrevious();
                break;
        }


    }
    void OnselectPress()
    {
  
        PlayerPrefs.SetInt("Level Select:", RoomIndex);
  
    }

    void UpdateInfo()
    {
        limitBoundsForIndex();

        infoTexts.text = "Room " + RoomIndex;


        PlayerPrefs.SetInt("PlayerIndex", RoomIndex);
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

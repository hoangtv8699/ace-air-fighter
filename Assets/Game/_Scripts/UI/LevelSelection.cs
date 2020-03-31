using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public class LevelSelection : MonoBehaviour {

    public Text infoTexts;
    private int LevelIndex;
    public int maxLevelIndex;
    public GameObject NextMenuObject;
    public GameObject selectionMenu;
    void OnEnable()
    {


        LevelIndex = 1;
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
        PlayerPrefs.SetInt("Level Select:", LevelIndex);

    }

    void UpdateInfo()
    {
        limitBoundsForIndex();

        infoTexts.text = "Level " + LevelIndex;


        PlayerPrefs.SetInt("PlayerIndex", LevelIndex);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Clickable))]
public class PlayButton : MonoBehaviour
{
    public GameObject difficultyMenuWrapper;
    private ButtonGroupAlphaControls difficultyButtons;
    public GameObject mainMenuWrapper;

    private ButtonGroupAlphaControls mainMenuButtons;
    private Clickable clickable;

    void Start()
    {
        clickable = GetComponent<Clickable>();
        difficultyButtons = difficultyMenuWrapper.GetComponent<ButtonGroupAlphaControls>();
        mainMenuButtons = mainMenuWrapper.GetComponent<ButtonGroupAlphaControls>();
    }

    public void onClick()
    {
        if (clickable.clickable)
        {
            difficultyButtons.displayAll();
            mainMenuButtons.hideAll();
            print("displaying difficulty menu");
        }
    }

}
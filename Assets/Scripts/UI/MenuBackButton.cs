using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackButton : MonoBehaviour
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
            difficultyButtons.hideAll();
            mainMenuButtons.displayAll();
        }
    }

}

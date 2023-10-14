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
        if(difficultyButtons == null)
            print("difficulty buttons null in " + gameObject.name);
        difficultyButtons = difficultyMenuWrapper.GetComponent<ButtonGroupAlphaControls>();
        mainMenuButtons = mainMenuWrapper.GetComponent<ButtonGroupAlphaControls>();
    }

    public void onClick()
    {
        print("menu back onclick");
        if (clickable.clickable)
        {
            difficultyButtons.hideAll();
            mainMenuButtons.displayAll();
        }
    }

}

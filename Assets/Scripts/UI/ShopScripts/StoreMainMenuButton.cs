using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Clickable))]
public class StoreMainMenuButton : MonoBehaviour
{
    public GameObject storeMenuWrapper;
    private ButtonGroupAlphaControls storeMenuButtons;
    public GameObject mainMenuWrapper;

    private ButtonGroupAlphaControls mainMenuButtons;
    private Clickable clickable;

    void Start()
    {
        clickable = GetComponent<Clickable>();
        storeMenuButtons = storeMenuWrapper.GetComponent<ButtonGroupAlphaControls>();
        mainMenuButtons = mainMenuWrapper.GetComponent<ButtonGroupAlphaControls>();
    }

    public void onClick()
    {
        if (clickable.clickable)
        {
            storeMenuButtons.displayAll();
            mainMenuButtons.hideAll();
        }
    }

}
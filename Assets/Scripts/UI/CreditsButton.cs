using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Clickable))]
public class CreditsButton : MonoBehaviour
{
    public GameObject creditsWrapper;
    private ButtonGroupAlphaControls creditsControls;
    public GameObject mainMenuWrapper;

    private ButtonGroupAlphaControls mainMenuControls;
    private Clickable clickable;

    void Start()
    {
        clickable = GetComponent<Clickable>();
        creditsControls = creditsWrapper.GetComponent<ButtonGroupAlphaControls>();
        mainMenuControls = mainMenuWrapper.GetComponent<ButtonGroupAlphaControls>();
    }

    public void onClick()
    {
        print("credits clicked");
        if (clickable.clickable)
        {
            creditsControls.displayAll();
            mainMenuControls.hideAll();
            print("displaying credits");
        }
    }

}

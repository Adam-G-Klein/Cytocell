using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingPopupController : MonoBehaviour
{
    public GameObject storePageWrapper;
    private ButtonGroupAlphaControls storePage;
    public ButtonGroupAlphaControls processingMenu;

    void Awake() {
        processingMenu = GetComponent<ButtonGroupAlphaControls>();
        if(storePageWrapper != null){
            storePage = storePageWrapper.GetComponent<ButtonGroupAlphaControls>();
        }
    }

    public void showProcessingMenu(){
        processingMenu.displayAll();
        storePage.hideAll();
    }

    public void hideProcessingMenu(){
        processingMenu.hideAll();
        storePage.displayAll();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProcessingPopupController : MonoBehaviour
{
    public GameObject storePageWrapper;
    private ButtonGroupAlphaControls storePage;
    public ButtonGroupAlphaControls processingMenu;
    public TextMeshProUGUI backText;
    public float backTextDelay = 5f;

    void Awake() {
        processingMenu = GetComponent<ButtonGroupAlphaControls>();
        if(storePageWrapper != null){
            storePage = storePageWrapper.GetComponent<ButtonGroupAlphaControls>();
        }
    }

    public void showProcessingMenu(){
        processingMenu.displayAll();
        storePage.hideAll();
        backText.gameObject.SetActive(false);
        Invoke("showBackText", backTextDelay);
    }

    private void showBackText() {
        backText.gameObject.SetActive(true);
    }

    public void hideProcessingMenu(){
        processingMenu.hideAll();
        storePage.displayAll();
        CancelInvoke("showBackText");
        backText.gameObject.SetActive(false);
    }

}

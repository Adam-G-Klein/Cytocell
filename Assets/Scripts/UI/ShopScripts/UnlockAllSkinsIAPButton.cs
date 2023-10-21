using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnlockAllSkinsIAPButton : MonoBehaviour
{

    public GameObject processingMenuGO;
    private ProcessingPopupController processingMenu;
    private ButtonGroupAlphaControls storePage;
    private TextMeshProUGUI buttonText;


    private void Start(){
        
        if(processingMenuGO != null){
            processingMenu = processingMenuGO.GetComponent<ProcessingPopupController>();
        }
        storePage = GetComponentInParent<ButtonGroupAlphaControls>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update() {
        if(PurchaseManager.instance.allSkinsUnlocked())
            buttonText.text = "Enjoy";
    }

    public void onButtonClick(){
        processingMenu.showProcessingMenu();
    }

    public void onSuccess() {
        //isn't getting called
    }

    public void onFetch() {
        //hideProcessingMenu();
        print("Fetch unlock all skins");
    }

    public void onFail() {
        //hideProcessingMenu();
        print("Fail unlock all skins");
    }

}

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
    private string initialText;


    private void Start(){
        
        if(processingMenuGO != null){
            processingMenu = processingMenuGO.GetComponent<ProcessingPopupController>();
        }
        storePage = GetComponentInParent<ButtonGroupAlphaControls>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        initialText = buttonText.text;
    }

    private void Update() {
        if(PurchaseManager.instance.allSkinsUnlocked())
            buttonText.text = "Enjoy";
        else 
            buttonText.text = initialText;
    }

    public void onButtonClick(){
        processingMenu.showProcessingMenu();
    }

    public void onSuccess() {
        //PurchaseManager.instance.purchaseSucceeded(InAppPurchases.ALL_SKINS_KEY);
        // handled by the iapListener on PurchaseManager
    }

    public void onFetch() {
        //PurchaseManager.instance.purchasesRestored();
        // handled by the iapListener on PurchaseManager
    }

    public void onFail() {
        // handled by the iapListener on PurchaseManager
        //PurchaseManager.instance.purchaseFailed();
    }

}

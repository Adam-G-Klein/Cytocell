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
        print("PurchaseManager.instance.allSkinsUnlocked() " + PurchaseManager.instance.allSkinsUnlocked());
        if(PurchaseManager.instance.allSkinsUnlocked())
            buttonText.text = "Enjoy";
        else 
            buttonText.text = initialText;
    }

    public void onButtonClick(){
        processingMenu.showProcessingMenu();
    }

    public void onSuccess() {
        print("onsuccess called");
        PurchaseManager.instance.purchaseSucceeded(InAppPurchases.ALL_SKINS_KEY);
        //isn't getting called
    }

    public void onFetch() {
        //hideProcessingMenu();
        print("Fetch unlock all skins");
        PurchaseManager.instance.purchasesRestored();
    }

    public void onFail() {
        //hideProcessingMenu();
        print("Fail unlock all skins");
        PurchaseManager.instance.purchaseFailed();
    }

}

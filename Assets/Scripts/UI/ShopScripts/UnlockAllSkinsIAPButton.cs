using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAllSkinsIAPButton : MonoBehaviour
{

    public GameObject processingMenuGO;
    private ButtonGroupAlphaControls processingMenu;
    private ButtonGroupAlphaControls storePage;

    private void Start(){
        
        if(processingMenuGO != null){
            processingMenu = processingMenuGO.GetComponent<ButtonGroupAlphaControls>();
        }
        storePage = GetComponentInParent<ButtonGroupAlphaControls>();
    }

    public void onButtonClick(){
        //showProcessingMenu();
    }

    private void showProcessingMenu(){
        storePage.hideAll();
        processingMenu.displayAll();
    }

    private void hideProcessingMenu(){
        StartCoroutine(hideProcessingMenuCoroutine());
    }

    private IEnumerator hideProcessingMenuCoroutine(){
        yield return new WaitForEndOfFrame();
        processingMenu.hideAll();
        storePage.displayAll();
    }

    public void onSuccess() {
        print("Unlock all skins");
        //hideProcessingMenu();
        PurchaseManager.instance.purchaseProduct(InAppPurchases.ALL_SKINS_KEY, () => {
            print("callback!");
            hideProcessingMenu();
        });
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

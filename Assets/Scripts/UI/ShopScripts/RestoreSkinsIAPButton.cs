using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreSkinsIAPButton : MonoBehaviour
{
    public GameObject processingMenuGO;
    private ProcessingPopupController processingMenu;

    void Start() {
        if(processingMenuGO != null){
            processingMenu = processingMenuGO.GetComponent<ProcessingPopupController>();
        }
    }
    public void onButtonClick(){
        print("attempting to restore purchases");
        processingMenu.showProcessingMenu();
    }

}

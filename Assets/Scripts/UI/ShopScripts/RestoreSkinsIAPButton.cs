using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreSkinsIAPButton : MonoBehaviour
{

    public void onButtonClick(){
        print("attempting to restore purchases");
        PurchaseManager.instance.iapRestorePurchases();
    }

}

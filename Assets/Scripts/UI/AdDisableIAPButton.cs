using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdDisableIAPButton : MonoBehaviour
{

    private InAppPurchases iap;
    void Start() {
        iap = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InAppPurchases>();
    }
    
    public void clicked(){
        iap.PurchaseDisableAds();
    }
}

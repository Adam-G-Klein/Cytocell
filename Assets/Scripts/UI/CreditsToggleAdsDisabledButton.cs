using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class CreditsToggleAdsDisabledButton : MonoBehaviour
{
    public static string DISCOUNT_DISABLE_ADS_KEY = "discountDisableAds";
    
    public void clicked(){
        print("Secret button clicked");
        PurchaseManager.instance.incrementCurrency(1000);
        PlayerPrefs.SetInt(DISCOUNT_DISABLE_ADS_KEY, 1);
    }
}

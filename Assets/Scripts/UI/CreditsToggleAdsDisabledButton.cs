using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsToggleAdsDisabledButton : MonoBehaviour
{
    public static string DISCOUNT_DISABLE_ADS_KEY = "discountDisableAds";
    
    public void clicked(){
        print("Secret button clicked");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(PurchaseManager.CURRENCY, 100000);
        PurchaseManager.instance.updateShopItems();
    }
}

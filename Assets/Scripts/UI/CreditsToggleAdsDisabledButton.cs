using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class CreditsToggleAdsDisabledButton : MonoBehaviour
{
    public static string DISCOUNT_DISABLE_ADS_KEY = "discountDisableAds";
    
    public void clicked(){
        print("Secret button clicked");
        PlayerPrefs.DeleteAll();
    }
}

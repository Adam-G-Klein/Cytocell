using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class CreditsToggleAdsDisabledButton : MonoBehaviour
{
    public static string DISCOUNT_DISABLE_ADS_KEY = "discountDisableAds";
    
    public void clicked(){
        print("credits ads disable clicked");
        if(PlayerPrefs.GetInt(AdsManager.ADS_DISABLED_KEY, 0) == 1){
            PlayerPrefs.SetInt(DISCOUNT_DISABLE_ADS_KEY, 0);
            PlayerPrefs.SetInt(AdsManager.ADS_DISABLED_KEY, 0);

        }
        else {
            PlayerPrefs.SetInt(DISCOUNT_DISABLE_ADS_KEY, 1);
            PlayerPrefs.SetInt(AdsManager.ADS_DISABLED_KEY, 1);
        }
    }
}

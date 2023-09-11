using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAdsDisabledButton : MonoBehaviour
{
    
    public void clicked(){
        if(PlayerPrefs.GetInt(AdsManager.ADS_DISABLED_KEY, 0) == 1)
            PlayerPrefs.SetInt(AdsManager.ADS_DISABLED_KEY, 0);
        else
            PlayerPrefs.SetInt(AdsManager.ADS_DISABLED_KEY, 1);
    }
}

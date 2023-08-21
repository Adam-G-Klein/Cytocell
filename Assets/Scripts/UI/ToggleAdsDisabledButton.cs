using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAdsDisabledButton : MonoBehaviour
{
    
    public void clicked(){
        if(PlayerPrefs.GetInt("adsRemoved", 0) == 1)
            PlayerPrefs.SetInt("adsRemoved", 0);
        else
            PlayerPrefs.SetInt("adsRemoved", 1);
    }
}

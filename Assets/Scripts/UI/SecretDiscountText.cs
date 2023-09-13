using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SecretDiscountText : MonoBehaviour
{
    private TextMeshProUGUI textComp;
    void Awake() {
        textComp = GetComponent<TextMeshProUGUI>();
        textComp.fontSize = 0f;
    }
    void Update() {
        if(PlayerPrefs.GetInt(CreditsToggleAdsDisabledButton.DISCOUNT_DISABLE_ADS_KEY, 0) == 1){
            textComp.fontSize = 36f;
        }else{
            textComp.fontSize = 0f;
        }
    }
}

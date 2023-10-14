using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalCurrencyDisplay : MonoBehaviour
{

    [SerializeField]
    protected TextMeshProUGUI statText; 

    void Awake() {
        statText = GetComponent<TextMeshProUGUI>();
    }

    public void updateStat(){
        print("update currency: " + PurchaseManager.instance.getCurrency());
        statText.text = TextUtils.intWithCommas(PurchaseManager.instance.getCurrency());
    }



}

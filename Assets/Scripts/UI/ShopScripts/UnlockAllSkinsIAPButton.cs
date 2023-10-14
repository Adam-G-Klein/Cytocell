using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAllSkinsIAPButton : MonoBehaviour
{

    public void onClick() {
        print("Unlock all skins");
        PurchaseManager.instance.iapUnlockAllSkins();
    }

}

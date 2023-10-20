using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class PurchaseManager : MonoBehaviour
{
    [SerializeField]
    private int currency;
    [SerializeField]
    private PlayerSkinSO equippedSkin;
    public static string UNLOCKED_SKINS = "unlockedSkins";
    public static string ALL_SKINS_UNLOCKED = "ALL_SKINS_UNLOCKED";
    public static string CURRENCY = "currency";
    public static string DEFAUKT_SKIN = "Alpha";
    public static PurchaseManager instance;
    private InAppPurchases inAppPurchases;

    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        inAppPurchases = GetComponent<InAppPurchases>();

    }

    public List<string> getUnlockedSkins()
    {
        return new List<string>(PlayerPrefs.GetString(UNLOCKED_SKINS, DEFAUKT_SKIN).Split(','));
    }

    public void unlockSkin(PlayerSkinSO skin)
    {
        List<string> unlockedSkins = getUnlockedSkins();
        unlockedSkins.Add(skin.name);
        PlayerPrefs.SetString(UNLOCKED_SKINS, string.Join(",", unlockedSkins.ToArray()));
    }

    public bool allSkinsUnlocked() {
        return PlayerPrefs.GetInt(ALL_SKINS_UNLOCKED, 0) == 1;
    }

    public bool isSkinUnlocked(PlayerSkinSO skin)
    {
        List<string> unlockedSkins = getUnlockedSkins();
        return unlockedSkins.Contains(skin.name) || allSkinsUnlocked();
    }

    public int getCurrency() {
        return PlayerPrefs.GetInt(CURRENCY, 0);
    }

    public void setCurrency(int newCurrency) {
        PlayerPrefs.SetInt(CURRENCY, newCurrency);
    }

    public void incrementCurrency(int amount) {
        PlayerPrefs.SetInt(CURRENCY, getCurrency() + amount);
    }

    public PlayerSkinSO getEquippedSkin() {
        return equippedSkin;
    }

    public void setEquippedSkin(PlayerSkinSO newSkin) {
        equippedSkin = newSkin;
    }

    public void purchaseProduct(string productId, Action callback) {
        print("purchase product: " + productId + " callback null? " + (callback == null));
        inAppPurchases.PurchaseProduct(productId, callback);
    }

    public void iapRestorePurchases() {
        inAppPurchases.RestorePurchase();
    }

    public void unlockAllSkins() {
        PlayerPrefs.SetInt(ALL_SKINS_UNLOCKED, 1);
    }



}

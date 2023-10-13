using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PurchaseManager : MonoBehaviour
{
    [SerializeField]
    private int currency;
    [SerializeField]
    private PlayerSkinSO equippedSkin;
    public static string UNLOCKED_SKINS = "unlockedSkins";
    public static string CURRENCY = "currency";
    public static PurchaseManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        currency = PlayerPrefs.GetInt(CURRENCY, 0);

    }

    public List<string> getUnlockedSkins()
    {
        return new List<string>(PlayerPrefs.GetString(UNLOCKED_SKINS).Split(','));
    }

    public void unlockSkin(PlayerSkinSO skin)
    {
        List<string> unlockedSkins = getUnlockedSkins();
        unlockedSkins.Add(skin.name);
        PlayerPrefs.SetString(UNLOCKED_SKINS, string.Join(",", unlockedSkins.ToArray()));
    }

    public int getCurrency() {
        return currency;
    }

    public void setCurrency(int newCurrency) {
        currency = newCurrency;
    }

    public PlayerSkinSO getEquippedSkin() {
        return equippedSkin;
    }

    public void setEquippedSkin(PlayerSkinSO newSkin) {
        equippedSkin = newSkin;
    }



}

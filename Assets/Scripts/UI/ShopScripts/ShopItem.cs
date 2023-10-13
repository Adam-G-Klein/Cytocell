using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Each display needs: 
- a demo player
- a price display
- a buy button
- a background
*/

/* This script: 
- Adds all of its components to the parent's ButtonGroupAlpaControls
- Handles equipping and unequipping skins via the purchaseManager
- Changes the "Buy, Use" button text based on whether the skin is equipped or not
- Hides and reveals the price of an item based on whether its been purchased
*/
public class ShopItem : MonoBehaviour
{
    public PlayerSkinSO playerSkinSO;
    public GameObject shopPlayer;
    // Start is called before the first frame update
    void Awake() {
        ButtonGroupAlphaControls parentMenu = GetComponentInParent<ButtonGroupAlphaControls>();
        foreach (Image image in GetComponentsInChildren<Image>()) {
            parentMenu.gameObjects.Add(image.gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

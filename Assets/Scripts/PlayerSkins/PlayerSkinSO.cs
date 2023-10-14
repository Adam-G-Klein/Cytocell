using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerSkin",
    menuName = "ScriptableObjects/PlayerSkin")]
public class PlayerSkinSO : ScriptableObject
{
    public Material playerMaterial;
    public Material shopMaterial;
    // 0: primary (membrane), 1: secondary (nucleus), 2: tertiary (swipe indicator / trail color )
    public List<Color> colors = new List<Color>();
    public GameObject idlePS;
    public GameObject swipePS;
    public GameObject deathPS;
    public float nucleusMovement;
    public float particleYOffset;

    public int price;

}
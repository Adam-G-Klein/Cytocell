using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinController : MonoBehaviour
{
    public PlayerSkinSO playerSkinSO;

    private ParticleSystem[] particleSystems;
    private PlayerManager pManage;
    private TrailLeaver trailLeaver;
    [SerializeField]
    private GameObject playerIdlePSPos;
    [SerializeField]
    private GameObject playerDeathPSPos;


    void Start() {
        playerSkinSO = PurchaseManager.instance.getEquippedSkin();
        Vector3 idlePSGO = playerIdlePSPos.transform.position;
        Vector3 idlePSPos = new Vector3(idlePSGO.x, idlePSGO.y + playerSkinSO.particleYOffset, idlePSGO.z);
        ParticleSystem idlePS = Instantiate(playerSkinSO.idlePS, idlePSPos, playerIdlePSPos.transform.rotation, playerIdlePSPos.transform).GetComponentInChildren<ParticleSystem>();
        ParticleSystem deathPS = Instantiate(playerSkinSO.deathPS, playerDeathPSPos.transform).GetComponentInChildren<ParticleSystem>();
        pManage = GetComponentInParent<PlayerManager>();
        if(pManage) {
            pManage.setDeathPS(deathPS);
            pManage.setIdlePS(idlePS);
            pManage.normalColor = playerSkinSO.playerMaterial.GetColor("_membraneColor");
        }

        trailLeaver = GetComponentInParent<TrailLeaver>();
        if(trailLeaver)
            trailLeaver.setSwipePS(playerSkinSO.swipePS);
        GetComponent<SpriteRenderer>().material = playerSkinSO.playerMaterial;
    }



}

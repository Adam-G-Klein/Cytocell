﻿using System.Collections;
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

    void Awake() {
        pManage = GetComponentInParent<PlayerManager>();
        ParticleSystem idlePS = Instantiate(playerSkinSO.idlePS, playerIdlePSPos.transform).GetComponentInChildren<ParticleSystem>();
        ParticleSystem deathPS = Instantiate(playerSkinSO.deathPS, playerDeathPSPos.transform).GetComponentInChildren<ParticleSystem>();
        pManage.setDeathPS(deathPS);
        pManage.setIdlePS(idlePS);
        trailLeaver = GetComponentInParent<TrailLeaver>();
        trailLeaver.setSwipePS(playerSkinSO.swipePS);



    }

    public void Start()
    {
        GetComponent<SpriteRenderer>().material = playerSkinSO.playerMaterial;

    }



}

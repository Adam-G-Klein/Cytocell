﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCameraEffect : MonoBehaviour
{
    [SerializeField]
    private Material _bloodLossMat;
    [SerializeField]
    private float maxEffect;
    private float maxHealth;
    [SerializeField]
    private float minEffect;

    public float effectVal;
    public bool clearEffect = false;

    private PlayerManager pManage;
    // Start is called before the first frame update
    void Start()
    {
        pManage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        maxHealth = pManage.maxHealth;
    }
    void Update()
    {

        effectVal = clearEffect || pManage.tweenedHealth == maxHealth ? 0 : maxEffect - ((maxEffect - minEffect) / maxHealth) * pManage.tweenedHealth;
        _bloodLossMat.SetFloat("_LossAmnt", effectVal);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination){
        Graphics.Blit(source,destination,_bloodLossMat);
    }
}

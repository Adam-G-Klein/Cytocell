﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeToMoveStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private PlayerSwiper swiper;
    private bool hasSwiped = false;
    private float waitTime = 1f;
    

    void Start() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        swiper = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSwiper>();
        hasSwiped = false;
    }

    void Update(){
        if(swiper.plSwiped && !hasSwiped) hasSwiped = true;
    }

    public void SwipeToMove(){
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        print("calling swipe to move display all");
        alphaControls.displayAll();
        yield return new WaitForSeconds(waitTime);
        yield return new WaitUntil(() => swiper.plSwiped);
        yield return new WaitForSeconds(1f);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}

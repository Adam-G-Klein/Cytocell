using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Analytics;

/*
If player has not swiped, and the playerPref isn't set, display text.

*/
public class SwipeToMoveStep : TutorialStep 
{
    private PlayerSwiper swiper;
    private bool hasSwiped = false;
    [SerializeField]
    private float waitTime = 1f;
    private GameManager gameManager;

    protected override void Start() {
        base.Start();
        stepName = "SwipeToMove";
        swiper = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSwiper>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public override IEnumerator executeStep(){
        yield return new WaitForSeconds(waitTime);
        alphaControls.displayAll();
        yield return new WaitUntil(() => swiper.plSwiped);
        yield return endExecution();
    }
}

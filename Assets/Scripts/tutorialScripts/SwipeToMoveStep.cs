using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

/*
If player has not swiped, and the playerPref isn't set, display text.

*/
public class SwipeToMoveStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private PlayerSwiper swiper;
    private bool hasSwiped = false;
    [SerializeField]
    private float waitTime = 1f;
    private GameManager gameManager;

    void Awake() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        swiper = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSwiper>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    public void resetTutorial(){
        PlayerPrefs.SetInt(TutorialManager.SWIPE_TO_MOVE_COMPLETION, 0);
        StopAllCoroutines();
        alphaControls.setVisibleQuickly(false);
        hasSwiped = false;
    }

    public void SwipeToMove(){
        if(PlayerPrefs.GetInt(TutorialManager.SWIPE_TO_MOVE_COMPLETION) == 1) {
            print("swipe to move already done");
            SendMessageUpwards("StepDone");
            return;
        } 
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        yield return new WaitUntil(() => !gameManager.gamePaused);
        yield return new WaitForSeconds(waitTime);
        if(!hasSwiped) {
            alphaControls.displayAll();
        } else {
            PlayerPrefs.SetInt(TutorialManager.SWIPE_TO_MOVE_COMPLETION, 1);
            SendMessageUpwards("StepDone");
            yield break;
        }
        yield return new WaitUntil(() => swiper.plSwiped);
        PlayerPrefs.SetInt("hasSwiped", 1);
        hasSwiped = true;
        yield return new WaitForSeconds(1f);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}

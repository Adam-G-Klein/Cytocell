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

    void Awake() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        swiper = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSwiper>();
        hasSwiped = false;

    }

    void Update(){
        if(swiper.plSwiped && !hasSwiped) hasSwiped = true;
    }

    public void SwipeToMove(){
        if(PlayerPrefs.GetInt("hasSwiped") == 1) {
            SendMessageUpwards("StepDone");
            return;
        } 
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        yield return new WaitForSeconds(waitTime);
        if(!hasSwiped) {
            alphaControls.displayAll();
        } else {
            PlayerPrefs.SetInt("hasSwiped", 1);
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

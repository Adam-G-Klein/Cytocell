using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MoveAcrossTrailStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private TrailCollapser collapser;
    private bool hasCollapsed = false;
    [SerializeField]
    private float waitTime = 1f;

    [SerializeField]
    private GameObject videoPlayer;
    [SerializeField]
    private GameObject videoBackground;
    private Material videoBackgroundMat;
    [SerializeField]
    private float videoBackgroundAlpha = 28f;
    [SerializeField]
    private GameObject pauseTint;
    private GameManager gameManager;
    private ButtonGroupAlphaControls buttonGroupAlphaControls;
    [SerializeField]
    public float videoFadeInTime = 0.05f; 

    void Awake() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        collapser = GameObject.FindGameObjectWithTag("Player").GetComponent<TrailCollapser>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        buttonGroupAlphaControls = GetComponent<ButtonGroupAlphaControls>();
        buttonGroupAlphaControls.displayTime = videoFadeInTime;
        alphaControls.displayTime = videoFadeInTime;
    }

    void Update() {
        if(collapser.collapseTriggered && !hasCollapsed) hasCollapsed = true;
    }

    public void MoveAcrossTrail(){
        /*
        if(PlayerPrefs.GetInt("hasCollapsed") == 1) {
            SendMessageUpwards("StepDone");
            return;
        } 
        */
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        yield return new WaitForSeconds(waitTime);
        alphaControls.displayAll();
        gameManager.setGamePaused(true, 0.1f);
        buttonGroupAlphaControls.displayAll();
        videoPlayer.SetActive(true);
        yield return new WaitUntil(() => Input.touchCount > 0);
        buttonGroupAlphaControls.hideAll();
        gameManager.setGamePaused(false);
        videoPlayer.SetActive(false);
        yield return new WaitUntil(() => collapser.collapseTriggered);
        PlayerPrefs.SetInt("hasCollapsed", 1);
        hasCollapsed = true;
        yield return new WaitForSeconds(1f);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}

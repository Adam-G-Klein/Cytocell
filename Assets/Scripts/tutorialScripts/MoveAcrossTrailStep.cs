using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MoveAcrossTrailStep : TutorialStep
{
    private TrailCollapser collapser;
    private bool hasCollapsed = false;
    [SerializeField]
    private float waitTime = 1f;

    [SerializeField]
    private GameObject videoPlayer;
    private GameManager gameManager;
    private ButtonGroupAlphaControls buttonGroupAlphaControls;
    [SerializeField]
    public float videoFadeInTime = 0.05f; 
    [SerializeField]
    public float gameTimeScaleWhilePlaying = 0.01f;
    [SerializeField]
    private GameObject textPromptGroupGO;
    private TextGroupAlphaControls textPromptGroupAlphaControls;
    [SerializeField]
    private float stepTextFadeOutTime = 0.7f;

    protected override void Start()
    {
        base.Start();
        stepName = "MoveAcrossTrail";
        collapser = GameObject.FindGameObjectWithTag("Player").GetComponent<TrailCollapser>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        buttonGroupAlphaControls = GetComponent<ButtonGroupAlphaControls>();
        textPromptGroupAlphaControls = textPromptGroupGO.GetComponent<TextGroupAlphaControls>();
    }

    public override void reset()
    {
        base.reset();
        initialize();
    }

    private void initialize() {
        buttonGroupAlphaControls.displayTime = videoFadeInTime;
        alphaControls.displayTime = videoFadeInTime;
        alphaControls.displayTime = videoFadeInTime;
        textPromptGroupAlphaControls.displayTime = videoFadeInTime;

    }

    public override IEnumerator executeStep(){
        print("started move across trail corout");
        initialize();
        yield return new WaitForSeconds(waitTime);
        yield return new WaitUntil(() => !gameManager.gamePaused);
        print("done waiting");
        alphaControls.displayAll();
        gameManager.setGamePaused(true, gameTimeScaleWhilePlaying);
        buttonGroupAlphaControls.displayAll();
        alphaControls.displayAll();
        textPromptGroupAlphaControls.displayAll();
        videoPlayer.SetActive(true);
        yield return new WaitUntil(() => Input.touchCount > 0);
        textPromptGroupAlphaControls.hideAll();
        buttonGroupAlphaControls.hideAll();
        gameManager.setGamePaused(false);
        videoPlayer.SetActive(false);
        yield return new WaitUntil(() => collapser.collapseTriggered);
        alphaControls.displayTime = stepTextFadeOutTime;
        yield return endExecution();
    }
}

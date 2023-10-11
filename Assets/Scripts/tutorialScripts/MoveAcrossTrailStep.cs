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
        textPromptGroupAlphaControls = textPromptGroupGO.GetComponent<TextGroupAlphaControls>();
        alphaControls.displayTime = stepTextFadeOutTime;
    }

    public override IEnumerator executeStep(){
        yield return new WaitForSeconds(waitTime);
        alphaControls.displayAll();
        yield return new WaitUntil(() => collapser.collapseTriggered);
        yield return endExecution();
    }
}

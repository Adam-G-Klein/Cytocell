using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAcrossTrailStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private TrailCollapser collapser;
    private bool hasCollapsed = false;

    void Start() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        collapser = GameObject.FindGameObjectWithTag("Player").GetComponent<TrailCollapser>();
    }

    void Update() {
        //if(collapser.collapseTriggered && !hasCollapsed) hasCollapsed = true;
    }

    public void MoveAcrossTrail(){
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        alphaControls.displayAll();
        yield return new WaitUntil(() => collapser.collapseTriggered);
        yield return new WaitForSeconds(1f);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}

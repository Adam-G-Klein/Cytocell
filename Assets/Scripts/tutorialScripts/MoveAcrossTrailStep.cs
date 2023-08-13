using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAcrossTrailStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private TrailCollapser collapser;
    private bool hasCollapsed = false;
    [SerializeField]
    private float waitTime = 1f;

    void Awake() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        collapser = GameObject.FindGameObjectWithTag("Player").GetComponent<TrailCollapser>();
    }

    void Update() {
        if(collapser.collapseTriggered && !hasCollapsed) hasCollapsed = true;
    }

    public void MoveAcrossTrail(){
        if(PlayerPrefs.GetInt("hasCollapsed") == 1) {
            SendMessageUpwards("StepDone");
            return;
        } 
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        yield return new WaitForSeconds(waitTime);
        if(!hasCollapsed) {
            alphaControls.displayAll();
        } else {
            PlayerPrefs.SetInt("hasCollapsed", 1);
            SendMessageUpwards("StepDone");
            yield break;
        }
        yield return new WaitUntil(() => collapser.collapseTriggered);
        PlayerPrefs.SetInt("hasCollapsed", 1);
        hasCollapsed = true;
        yield return new WaitForSeconds(1f);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}

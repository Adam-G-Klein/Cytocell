using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnjoyStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private bool hasEnjoyed = false;
    [SerializeField]
    private float waitTime = 1f;
    [SerializeField]
    private float displayTime = 1f;

    void Awake() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
    }

    public void resetTutorial(){
        PlayerPrefs.SetInt(TutorialManager.HAS_ENJOYED_COMPLETION, 0);
        hasEnjoyed = false;
        StopAllCoroutines();
        alphaControls.setVisibleQuickly(false);
    }

    public void Enjoy(){
        if(PlayerPrefs.GetInt(TutorialManager.HAS_ENJOYED_COMPLETION) == 1) {
            SendMessageUpwards("StepDone");
            return;
        } 
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        yield return new WaitForSeconds(waitTime);
        alphaControls.displayAll();
        PlayerPrefs.SetInt(TutorialManager.HAS_ENJOYED_COMPLETION, 1);
        hasEnjoyed = true;
        yield return new WaitForSeconds(displayTime);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}


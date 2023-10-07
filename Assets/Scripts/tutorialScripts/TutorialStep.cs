using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class TutorialStep : MonoBehaviour {

    [SerializeField]
    public string stepName;
    protected TextGroupAlphaControls alphaControls;

    private const float END_EXECUTION_WAIT_TIME = 1f;

    protected virtual void Start() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
    }

    public abstract IEnumerator executeStep();

    public virtual IEnumerator endExecution() {
        yield return new WaitForSeconds(END_EXECUTION_WAIT_TIME);
        alphaControls.hideAll();
        PlayerPrefs.SetInt(completionKey(), 1);
    }

    public virtual void reset(){
        Debug.Log("Resetting step " + stepName);
        StopCoroutine("executeStep");
        PlayerPrefs.SetInt(completionKey(), 0);
        alphaControls.setVisibleQuickly(false);
    }

    public virtual string completionKey(){
        return stepName + "-done";
    }

    public virtual void setVisible(bool val){
        alphaControls.setVisibleQuickly(val);
    }

}

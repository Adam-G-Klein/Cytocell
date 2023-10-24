using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class TutorialStep : MonoBehaviour {

    [SerializeField]
    public string stepName;
    protected TextGroupAlphaControls alphaControls;

    private const float END_EXECUTION_WAIT_TIME = 1f;

    public abstract string getStepName();

    protected virtual void Awake() {
        stepName = getStepName();
    }

    protected virtual void Start() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
    }

    public abstract IEnumerator executeStep();

    public virtual IEnumerator endExecution() {
        alphaControls.hideAll();
        PlayerPrefs.SetInt(completionKey(), 1);
        yield return new WaitForSeconds(END_EXECUTION_WAIT_TIME);
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

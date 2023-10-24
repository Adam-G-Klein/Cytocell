using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnjoyStep : TutorialStep
{
    [SerializeField]
    private float waitTime = 1f;
    [SerializeField]
    private float displayTime = 1f;

    protected override void Start() {
        base.Start();
    }

    public override IEnumerator executeStep(){
        yield return new WaitForSeconds(waitTime);
        alphaControls.displayAll();
        yield return new WaitForSeconds(displayTime);
        yield return endExecution();
    }

    public override string getStepName(){
        return "Enjoy";
    }
}


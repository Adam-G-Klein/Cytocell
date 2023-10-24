using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeCellsStep : TutorialStep
{
    private GameManager manager;
    private bool hasConsumed = false;
    [SerializeField]
    private float waitTime = 1f;

    protected override void Start()
    {
        base.Start();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        hasConsumed = false;
    }

    public override IEnumerator executeStep(){
        yield return new WaitForSeconds(waitTime);
        alphaControls.displayAll();
        int startingKillCount = manager.score;
        yield return new WaitUntil(() => manager.score > startingKillCount);
        yield return endExecution();
    }


    public override string getStepName(){
        return "ConsumeCells";
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpStep : TutorialStep
{
    private XPManager manager;
    private bool hasLeveled = false;
    [SerializeField]
    private float waitTime = 1f;

    protected override void Start() {
        base.Start();
        manager = GameObject.FindGameObjectWithTag("Player").GetComponent<XPManager>();
    }

    public override IEnumerator executeStep(){
        print("started level up corout");
        int startingLevel = manager.level;
        yield return new WaitForSeconds(waitTime);
        print("level done waitng");
        alphaControls.displayAll();
        yield return new WaitUntil(() => manager.level > startingLevel);
        PlayerPrefs.SetInt(TutorialManager.LEVEL_UP_COMPLETION, 1);
        yield return endExecution();
    }

    public override string getStepName(){
        return "LevelUp";
    }
}


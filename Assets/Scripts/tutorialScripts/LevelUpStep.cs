using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private XPManager manager;
    private bool hasLeveled = false;
    [SerializeField]
    private float waitTime = 1f;

    void Awake() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        manager = GameObject.FindGameObjectWithTag("Player").GetComponent<XPManager>();
    }

    public void resetTutorial(){
        PlayerPrefs.SetInt(TutorialManager.LEVEL_UP_COMPLETION, 0);
        hasLeveled = false;
        StopAllCoroutines();
        alphaControls.setVisibleQuickly(false);
    }

    public void LevelUp(){
        if(PlayerPrefs.GetInt(TutorialManager.LEVEL_UP_COMPLETION) == 1) {
            SendMessageUpwards("StepDone");
            return;
        } 
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        print("started level up corout");
        int startingLevel = manager.level;
        yield return new WaitForSeconds(waitTime);
        alphaControls.displayAll();
        yield return new WaitUntil(() => manager.level > startingLevel);
        PlayerPrefs.SetInt(TutorialManager.LEVEL_UP_COMPLETION, 1);
        hasLeveled = true;
        yield return new WaitForSeconds(1f);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}


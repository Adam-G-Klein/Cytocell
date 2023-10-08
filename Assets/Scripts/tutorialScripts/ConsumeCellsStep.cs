using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeCellsStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private GameManager manager;
    private bool hasConsumed = false;
    [SerializeField]
    private float waitTime = 1f;
    

    void Awake() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        hasConsumed = false;
    }

    public void ConsumeCells(){
        if(PlayerPrefs.GetInt(TutorialManager.CONSUME_CELLS_COMPLETION) == 1) {
            SendMessageUpwards("StepDone");
            return;
        } 
        StartCoroutine("corout");
    }

    public void resetTutorial(){
        print("resetting consume cells step");
        PlayerPrefs.SetInt(TutorialManager.CONSUME_CELLS_COMPLETION, 0);
        StopAllCoroutines();
        alphaControls.setVisibleQuickly(false);
        hasConsumed = false;
    }

    private IEnumerator corout(){
        yield return new WaitForSeconds(waitTime);
        hasConsumed = manager.currentPurgeKillCount > 0;
        if(!hasConsumed) {
            alphaControls.displayAll();
        } else {
            PlayerPrefs.SetInt(TutorialManager.CONSUME_CELLS_COMPLETION, 1);
            SendMessageUpwards("StepDone");
            yield break;
        }
        yield return new WaitUntil(() => manager.currentPurgeKillCount> 0);
        PlayerPrefs.SetInt(TutorialManager.CONSUME_CELLS_COMPLETION, 1);
        hasConsumed = true;
        yield return new WaitForSeconds(1f);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}


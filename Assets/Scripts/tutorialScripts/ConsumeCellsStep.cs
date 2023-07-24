using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeCellsStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private GameManager manager;
    

    void Start() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void ConsumeCells(){
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        alphaControls.displayAll();
        yield return new WaitUntil(() => manager.currentPurgeKillCount > 0);
        yield return new WaitForSeconds(1f);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}


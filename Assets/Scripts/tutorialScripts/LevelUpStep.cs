using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private XPManager xPManager;
    private DifficultyConstants constants;

    void Awake() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        xPManager = GameObject.FindGameObjectWithTag("Player").GetComponent<XPManager>();
    }

    void Update() {
    }

    public void LevelUp(){
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        alphaControls.displayAll();
        yield return new WaitUntil(() => xPManager.xp >= xPManager.nextLevelXP);
        yield return new WaitForSeconds(1f);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}


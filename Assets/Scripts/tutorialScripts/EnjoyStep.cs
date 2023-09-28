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

    public void Enjoy(){
        if(PlayerPrefs.GetInt("hasEnjoyed") == 1) {
            SendMessageUpwards("StepDone");
            return;
        } 
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        yield return new WaitForSeconds(waitTime);
        alphaControls.displayAll();
        PlayerPrefs.SetInt("hasEnjoyed", 1);
        hasEnjoyed = true;
        yield return new WaitForSeconds(displayTime);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}


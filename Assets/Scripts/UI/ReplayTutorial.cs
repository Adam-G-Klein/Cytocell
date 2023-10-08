using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ReplayTutorial : MonoBehaviour
{
    private TutorialManager tutorialManager;
    void Start() {
        tutorialManager = GameObject.FindGameObjectWithTag("TutorialCanvas").GetComponent<TutorialManager>();


    }
    public void click(){
        tutorialManager.resetAllSteps();
        tutorialManager.startTutorial();
    }

}

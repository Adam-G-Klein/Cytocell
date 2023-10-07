using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using TMPro;


public class TutorialManager : MonoBehaviour
{

    // Step 1: list of TutorialStep objects that all have an executeStep iEnumerable. Compiles.
    // Step 2: implement one tutorial step, execute it from tutorialManager
    // Step 3: TutorialManager tracks the current TutorialStep and the coroutine we're currently yielding on. Calling reset:
    //              - cancels the current coroutine
    //              - calls reset on all TutorialSteps
    //              - Restarts the first coroutine
    public static string SWIPE_TO_MOVE_COMPLETION = "hasSwiped";
    public static string MOVE_ACROSS_TRAIL_COMPLETION = "hasCollapsed";
    public static string CONSUME_CELLS_COMPLETION = "hasConsumed";
    public static string LEVEL_UP_COMPLETION = "hasLeveled";
    public static string HAS_ENJOYED_COMPLETION = "hasEnjoyed";
    public static string[] STEP_COMPLETION_KEYS = {SWIPE_TO_MOVE_COMPLETION, MOVE_ACROSS_TRAIL_COMPLETION, CONSUME_CELLS_COMPLETION, LEVEL_UP_COMPLETION, HAS_ENJOYED_COMPLETION};

    public List<TutorialStep> tutorialSteps;

    private IEnumerator currentTutorialCoroutine;
    private IEnumerator currentStepCoroutine;
    private TutorialStep currentStep;
    private GameManager gameManager;
    [SerializeField]
    private string initStepName;
    private IEnumerator resetCoroutine;

    void Start() {
        tutorialSteps = new List<TutorialStep>(GetComponentsInChildren<TutorialStep>());
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        print("tutorialSteps: " + tutorialSteps.Count);
        startTutorial();
    }

    public void resetTutorial(){
        if (resetCoroutine != null) {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = resetTutorialAfterUnpause();
        StartCoroutine(resetCoroutine);
    }

    private IEnumerator resetTutorialAfterUnpause() {
        yield return new WaitUntil(() => !gameManager.gamePaused);
        if(currentTutorialCoroutine != null) {
            StopCoroutine(currentTutorialCoroutine);
        }
        if(currentStepCoroutine != null) {
            StopCoroutine(currentStepCoroutine);
        }
        foreach(TutorialStep step in tutorialSteps) {
            step.reset();
        }
        startTutorial();
    }

    public void startTutorial(){
        currentTutorialCoroutine = tutorialCoroutine();
        StartCoroutine(currentTutorialCoroutine);
    }

    public void setVisible(bool visible) {
        if(currentStep != null)
            currentStep.setVisible(visible);
    }

    private IEnumerator tutorialCoroutine() {
        bool startingStepFound = false;
        foreach(TutorialStep step in tutorialSteps) {
            startingStepFound = startingStepFound || step.stepName == initStepName;
            if(startingStepFound && PlayerPrefs.GetInt(step.completionKey(), 0) == 0) {
                print("Starting step " + step.stepName);
                currentStepCoroutine = step.executeStep();
                currentStep = step;
                yield return StartCoroutine(currentStepCoroutine);
            }
        }
    }

}

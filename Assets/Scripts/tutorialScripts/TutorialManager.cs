using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using TMPro;

public class TextCommand
{
    // command: ~ xCoord yCoord beforeDelay duringDelay afterDelay text
    public float beforeDelay;
    public float duringDelay;
    public float afterDelay;

    public string text;
    public float xCoord;
    public float yCoord;


    public TextCommand()
    {
        beforeDelay = 0;
        duringDelay = 1;
        afterDelay = 1;
        text = "boooo unset text!";
        xCoord = 0;
        yCoord = 0;
    }
}

public class TutorialManager : MonoBehaviour
{
    // command: ~ xCoord yCoord beforeDelay duringDelay afterDelay text
    [SerializeField]
    private int initStep = 0;
    public string[] stepNames;
    public int stepCounter = 0;
    private int stepsInitialized = 0;
    private string currentStep;
    public GameObject textPrefab;
    public GenericTextStep genericText;
    void Start()
    {
        stepCounter = initStep;
        processStep(stepNames[stepCounter]);
        genericText = GetComponentInChildren<GenericTextStep>();
    }

    //last step in the chain won't send upwards for this function to be called
    public void StepDone()
    {
        stepCounter += 1;
        if (stepCounter < stepNames.Length)
        {
            currentStep = stepNames[stepCounter];
            processStep(currentStep);
        }
    }

    private void processStep(string step)
    {
        string[] stepSplit = step.Split();
        if (step.Split()[0] == "~")
        {
            TextCommand cmd = parseTextCommand(step);
            StartCoroutine("executeTextCommand", cmd);
        } else {
            BroadcastMessage(stepSplit[0]);
        }
    }

    private TextCommand parseTextCommand(string currentStep)
    {
        string[] tokens = currentStep.Split();
        TextCommand cmd = new TextCommand();
        cmd.xCoord = float.Parse(tokens[1]);
        cmd.yCoord = float.Parse(tokens[2]);
        cmd.beforeDelay = float.Parse(tokens[3]);
        cmd.duringDelay = float.Parse(tokens[4]);
        cmd.afterDelay = float.Parse(tokens[5]);
        StringBuilder sb = new StringBuilder(tokens[6], 500);
        int cnt = 7;
        while (cnt < tokens.Length)
        {
            sb.Append(" " + tokens[cnt]);
            cnt += 1;
        }
        cmd.text = sb.ToString();
        return cmd;
    }


    IEnumerator executeTextCommand(TextCommand cmd)
    {
        yield return new WaitForSeconds(cmd.beforeDelay);
        genericText.displayText(cmd.duringDelay, cmd.text);
        yield return new WaitForSeconds(cmd.duringDelay + cmd.afterDelay);
        StepDone();
    }

}

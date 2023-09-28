using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextGroupAlphaControls : MonoBehaviour
{

    //public List<TextMeshProUGUI> texts;
    public float displayTime = 0.7f;
    private TextMeshProUGUI[] textComps;
    public float initAlpha = 0;
    private bool testDisplaying = false;
    private int ltid = -1;
    private bool displaying = false;

    // Start is called before the first frame update
    void Awake()
    {
        textComps = transform.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI tm in textComps)
        {
            tm.alpha = initAlpha;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (testDisplaying)
            {
                testDisplaying = false;
                hideAll();
            }
            else
            {
                testDisplaying = true;
                displayAll();
            }
        }
    }

    public void setVisibleQuickly(bool visible){
        StopAllCoroutines();
        if(ltid != -1 && LeanTween.isTweening(ltid)){
            LeanTween.cancel(ltid);
        }
        foreach(TextMeshProUGUI tm in textComps){
            tm.faceColor = new Color(tm.faceColor.r, tm.faceColor.g, tm.faceColor.b, visible ? 1 : 0);
        }
        if(visible && displaying) {
            displayAll();
        }

    }

    public void displayAll()
    {
        if(ltid != -1 && LeanTween.isTweening(ltid))
            LeanTween.cancel(ltid);
        StopAllCoroutines();
        StartCoroutine(tweenAlphaTo(1, displayTime));
        displaying = true;
    }

    public void hideAll()
    {
        if(ltid != -1 && LeanTween.isTweening(ltid))
            LeanTween.cancel(ltid);
        StopAllCoroutines();
        StartCoroutine(tweenAlphaTo(0, displayTime));
        displaying = false;
    }

    private IEnumerator tweenAlphaTo(float to, float time)
    {
        foreach (TextMeshProUGUI tm in textComps)
        {
            LeanTween.value(
                gameObject, tm.alpha, to, time)
                .setOnUpdate((float val) =>
                {
                    tm.color = new Color(tm.faceColor.r, tm.faceColor.g, tm.faceColor.b, val);
                });
        }
        yield return new WaitForSeconds(time);
        displaying = false;

    }

}


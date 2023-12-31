﻿using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
public class XPBarController : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private LevelPSController levelPs; 
    [SerializeField]
    private float locMaxXPPoints = 100;
    [SerializeField]
    private float stepsLength = 1;


    private float locGrowingXPvalue;

    private RectTransform imageRectTransform;

    private float locFutureXP;
    private float locRealXPvalue;

    int ltidFuture = 0;
    int ltidSetPoint = 0;
    int ltidBarAnimate = 0;
    int ltidAppearance = 0;

    [SerializeField]
    public float XPDisplayTeaseTime = 10;

    [SerializeField]
    public float XPDisplayLevelUpTime = 1;

   


    //this is the value that represents the real current amount of
    //xp in the bar, not the one being displayed
    public float realXPvalue
    {
        get { return locRealXPvalue; }
        set
        {
            locRealXPvalue = Mathf.Clamp(value, 0, maxXpPoints);
            image.material.SetFloat("_realXPpercent", locRealXPvalue / maxXpPoints);
        }


    }
    public float growingXPvalue
    {
        get { return locGrowingXPvalue; }
        set
        {
            locGrowingXPvalue = Mathf.Clamp(value, 0, maxXpPoints);
            //the _percent variable is the xp shown
            image.material.SetFloat("_growingXPpercent", locGrowingXPvalue / maxXpPoints);

            if (locGrowingXPvalue < Mathf.Epsilon)
                locGrowingXPvalue = 0;
        }
    }

    public float futureXPpercent
    {
        get { return locFutureXP; }
        set
        {
            locFutureXP = Mathf.Clamp(value, 0, maxXpPoints);
            //
            image.material.SetFloat("_futureXPpercent", locFutureXP / maxXpPoints);
        }
    }

    public float maxXpPoints
    {
        get { return locMaxXPPoints; }
        set
        {
            locMaxXPPoints = value;
            image.material.SetFloat("_Steps", maxXpPoints / stepsLength);
        }
    }

    //the alpha that changes the border on the bar
    public float borderAlpha
    {
        get { return locMinAlpha; }
        set
        {
            locMinAlpha = value;
            image.material.SetFloat("_MinAlpha", locMinAlpha);
        }
    }
    //the alpha applied to the whole bar, used to appear and disappear it
    public float barAlpha
    {
        get { return locBarAlpha; }
        set
        {
            locBarAlpha = value;
            image.material.SetFloat("_publicAlpha", locBarAlpha);
        }
    }
    //next six vars are the attributes of the xp bar animation 
    [SerializeField]
    private bool minAlphaHigh = false;
    [SerializeField]
    private float minAlphaAddSubRange = 0.3f;
    [SerializeField]
    private float minAlphaAnimSpeedBase = 2f;
    [SerializeField]
    private float minAlphaAnimSpeedRange = 1f;
    [SerializeField]
    private float minMinAlpha = 0.15f;
    //attributes of xp bar appearing and disappearing
    [SerializeField]
    private float barAppearTime = 0.5f;
    [SerializeField]
    private float barDisappearTime = 0.5f;
    [SerializeField]
    private float barIdleTime = 2f;
    private bool barVisible = false;

    private float locBarAlpha = 0f;
    private float locMinAlpha = 0f;

    private VisualManager vis;
    protected void Awake()
    {
        vis = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<VisualManager>();
        imageRectTransform = image.GetComponent<RectTransform>();
        image.material = Instantiate(image.material); // Clone material

        image.material.SetVector("_ImageSize", new Vector4(imageRectTransform.rect.size.x, imageRectTransform.rect.size.y, 0, 0));

        maxXpPoints = maxXpPoints; // Force the call to the setters in order to update the material
        growingXPvalue = 0; 
        futureXPpercent = 0;
        realXPvalue = 0; 
        barAlpha = 0;
        borderAlpha = minMinAlpha;
        levelPs = GetComponentInChildren<LevelPSController>();

    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            barAppear();
            levelUpAnimate();
        }
        if (barVisible)
        {
            animate();
        }



    }

    private void animate()
    {
        if (ltidBarAnimate == 0 || !LeanTween.isTweening(ltidBarAnimate))
        {
            float nextMinAlpha;
            float randModAmnt = UnityEngine.Random.Range(0, minAlphaAddSubRange);
            float minAlphaAnimSpeed = minAlphaAnimSpeedBase + UnityEngine.Random.Range(-minAlphaAnimSpeedRange, minAlphaAnimSpeedRange);
            if (minAlphaHigh)
            {
                nextMinAlpha = 1 - randModAmnt;
                minAlphaHigh = false;
            }
            else
            {
                nextMinAlpha = minMinAlpha + randModAmnt;
                minAlphaHigh = true;
            }

            ltidBarAnimate = LeanTween.value(gameObject, updateMinAlpha, borderAlpha, nextMinAlpha, minAlphaAnimSpeed).id;

        }
    }
    public void barAppear()
    {
        StopCoroutine("barAppearCorout");
        StartCoroutine("barAppearCorout");
    }

    public void setXPWithTween(float newXPval)
    {

        realXPvalue = newXPval;
        if (ltidSetPoint != 0 && LeanTween.isTweening(ltidSetPoint))
        {
            LeanTween.cancel(ltidSetPoint);
        }

        ltidSetPoint = LeanTween.value(gameObject, growingXPvalue, realXPvalue, XPDisplayTeaseTime).setOnUpdate((float val) => { growingXPvalue = val; }).id;
    }

    public void setXPNoTween(float newXPval)
    {

        realXPvalue = growingXPvalue = newXPval;
        if (ltidSetPoint != 0 && LeanTween.isTweening(ltidSetPoint))
        {
            LeanTween.cancel(ltidSetPoint);
        }
    }
    public void giveXP(float xpAdded)
    {
        realXPvalue += xpAdded;
        if (ltidSetPoint != 0 && LeanTween.isTweening(ltidSetPoint))
        {
            LeanTween.cancel(ltidSetPoint);
        }

        ltidSetPoint = LeanTween.value(gameObject, growingXPvalue, realXPvalue, XPDisplayTeaseTime).setOnUpdate((float val) => { growingXPvalue = val; }).id;
    }
    private IEnumerator barAppearCorout()
    {
        barVisible = true;
        if (ltidAppearance != 0)
        {
            LeanTween.cancel(ltidAppearance);
        }
        ltidAppearance = LeanTween.value(gameObject, updateAppearanceAlpha, barAlpha, 1, (1 - barAlpha) * barAppearTime).id;
        yield return new WaitForSeconds(10);
        ltidAppearance = LeanTween.value(gameObject, updateAppearanceAlpha, barAlpha, 0, barAlpha * barDisappearTime).id;
        barVisible = false;
        yield return null;
    }
    private void updateAppearanceAlpha(float val)
    {
        barAlpha = val;
    }
    private void updateMinAlpha(float val)
    {
        borderAlpha = val;
    }

    public void levelUpAnimate()
    {

        StartCoroutine("levelUpAnimation");
    }
    public IEnumerator levelUpAnimation()
    {

        vis.displayLevelText();
        levelPs.emit();
        yield return new WaitForSeconds(XPDisplayLevelUpTime); 
    }

}







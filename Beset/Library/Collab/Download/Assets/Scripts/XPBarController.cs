using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
public class XPBarController : MonoBehaviour
{
    [SerializeField]
    private Image image ;

    [SerializeField]
    private float locMaxXPPoints = 100;
    [SerializeField]
    private float stepsLength = 1;

    [SerializeField]
    private float growingXPGrowthTime = 10;
    [SerializeField]
    private float teaserXPGrowthTime = 1;

    private float locGrowingXPvalue ;

    private RectTransform imageRectTransform ;

    private float locFutureXP;
    private float locRealXPvalue;
    public delegate void levelUpDelegate();

    int ltidFuture = 0;
    int ltidGrowingValue = 0; 
    int ltidTeaserValue = 0; 
    int ltidBarAnimate = 0;
    int ltidAppearance = 0;

    //this is the value that represents the real current amount of
    //xp in the bar, not the one being displayed
    public float teaserXPvalue
    {
        get{ return locRealXPvalue;}
        set
        {
            //print("in the realXP getter");
                locRealXPvalue = Mathf.Clamp(value, 0, maxXpPoints);
                //from original code
                //the _percent variable is the xp shown
                image.material.SetFloat("_realXPpercent", locRealXPvalue / maxXpPoints);
        }


    }
    public float growingXPvalue
    {
        get { return locGrowingXPvalue; }
        set
        {
            locGrowingXPvalue = Mathf.Clamp(value, 0, maxXpPoints);
            //from original code
            //the _percent variable is the xp shown
            image.material.SetFloat("_growingXPpercent", locGrowingXPvalue / maxXpPoints);

            if (locGrowingXPvalue < Mathf.Epsilon)
            locGrowingXPvalue = 0;
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
    public float borderAlpha{
        get{return locMinAlpha;}
        set{
            locMinAlpha = value;
            image.material.SetFloat("_MinAlpha", locMinAlpha);
        }
    }
    //the alpha applied to the whole bar, used to appear and disappear it
    public float barAlpha{
        get{return locBarAlpha;}
        set{
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
    protected void Awake()
    {
        imageRectTransform = image.GetComponent<RectTransform>();
        image.material = Instantiate(image.material); // Clone material

        image.material.SetVector( "_ImageSize", new Vector4( imageRectTransform.rect.size.x, imageRectTransform.rect.size.y, 0, 0) );

        maxXpPoints = maxXpPoints; // Force the call to the setter in order to update the material
        growingXPvalue = 0; // Force the call to the setter in order to update the material
        teaserXPvalue = 0; // Force the call to the setter in order to update the material
        barAlpha = 0;
        borderAlpha = minMinAlpha;
        
    }

    protected void Update()
    {
        /* if( XPSetPoint < realXPValue)
        {
        futureXPShown -= XPDisplayIncreaseRate * Time.deltaTime;
        XPSetPoint += XPDisplayIncreaseRate * Time.deltaTime;

        }*/
        if(Input.GetKeyDown(KeyCode.Space)){
            barAppear();
        }
        if(barVisible){
            animate();
        }
    }

    private void animate(){
        if(ltidBarAnimate == 0 || !LeanTween.isTweening(ltidBarAnimate)){
            float nextMinAlpha;
            float randModAmnt = UnityEngine.Random.Range(0 , minAlphaAddSubRange);
            float minAlphaAnimSpeed = minAlphaAnimSpeedBase + UnityEngine.Random.Range(-minAlphaAnimSpeedRange, minAlphaAnimSpeedRange);
            if(minAlphaHigh){
                nextMinAlpha = 1 - randModAmnt;
                minAlphaHigh = false;
            }
            else{
                nextMinAlpha = minMinAlpha + randModAmnt;
                minAlphaHigh = true;
            }

            ltidBarAnimate = LeanTween.value(gameObject, updateMinAlpha, borderAlpha, nextMinAlpha, minAlphaAnimSpeed).id;

        }
    }
    public void barAppear(){
        StopCoroutine("barAppearCorout");
        StartCoroutine("barAppearCorout");
    }
    
    public int setXPDisplayed(float newXPval, float newMaxPoints){
        
        teaserXPvalue = (teaserXPvalue / maxXpPoints) * newMaxPoints;
        growingXPvalue = (growingXPvalue / maxXpPoints) * newMaxPoints;
        maxXpPoints = newMaxPoints;//used to calculate how the other values are displayed
        if(ltidGrowingValue != 0 && LeanTween.isTweening(ltidGrowingValue)){
            LeanTween.cancel(ltidGrowingValue);
        }
        if(ltidTeaserValue != 0 && LeanTween.isTweening(ltidTeaserValue)){
            LeanTween.cancel(ltidTeaserValue);
        }
        
//        print("tweening teaser from " + teaserXPvalue + " to " + newXPval + " where maxXp is " + maxXpPoints);
        ltidTeaserValue = LeanTween.value(gameObject, teaserXPvalue, newXPval, teaserXPGrowthTime).setOnUpdate((float val)=>{teaserXPvalue = val;}).id;
        ltidGrowingValue = LeanTween.value(gameObject, growingXPvalue, newXPval, growingXPGrowthTime).setOnUpdate((float val)=>{growingXPvalue = val;}).id;
        return ltidGrowingValue;



    }
    /*
    public void levelUp(levelUpDelegate levelUpCallBack){
        StartCoroutine("levelUpCorout", levelUpCallBack);
    }
    */
    /*
    public void giveXP(float xpAdded){
        //print("inside bar give xp func, added " + xpAdded);
        teaserXPvalue += xpAdded;
        if(ltidSetPoint != 0 && LeanTween.isTweening(ltidSetPoint)){
            //LeanTween.cancel(ltidFuture);
            LeanTween.cancel(ltidSetPoint);
            //futureXPShown = realXPValue;
        }

        ltidSetPoint = LeanTween.value(gameObject, growingXPvalue, teaserXPvalue, XPDisplayIncreaseRate).setOnUpdate((float val)=>{growingXPvalue = val;}).id;
        //ltidFuture = LeanTween.value(gameObject, realXPValue, growingXPpercent, XPDisplayIncreaseRate).setOnUpdate((float val)=>{futureXPpercent = val;}).id;
    }
    */
    private IEnumerator barAppearCorout(){
        barVisible = true;
        if(ltidAppearance != 0){
            LeanTween.cancel(ltidAppearance);
        }
        ltidAppearance = LeanTween.value(gameObject, updateAppearanceAlpha, barAlpha, 1, (1 - barAlpha) * barAppearTime).id;
        yield return new WaitForSeconds(10);
        ltidAppearance = LeanTween.value(gameObject, updateAppearanceAlpha, barAlpha, 0, barAlpha * barDisappearTime).id;
        barVisible = false;
        yield return null;
    }
    private void updateAppearanceAlpha(float val){
        barAlpha = val;
    }
    private void updateMinAlpha(float val){
        borderAlpha = val;
    }
    private IEnumerator levelUpCorout(levelUpDelegate cb){
        setXPDisplayed(maxXpPoints,maxXpPoints);//weird, but just need xp to equal maxXp in the display
        yield return new WaitForSeconds(growingXPGrowthTime);
        teaserXPvalue = 0;
        growingXPvalue = 0;
        cb();

    }
}







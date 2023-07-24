using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    [SerializeField]
    private int xp = 0;
    [SerializeField]
    private int flitXPVal;

    [SerializeField]
    private int level = 0;
    [SerializeField]
    private int maxTrails;
    [SerializeField]
    private int initMaxTrails;
    [SerializeField]
    private int maxTrailsInc;


    [SerializeField]
    private float swipeTime;
    [SerializeField]
    private float initSwipeTime;
    [SerializeField]
    private float swipeTimeDec;

    [SerializeField]
    public float regenTime;
    [SerializeField]
    private float initRegenTime;
    [SerializeField]
    private float regenTimeInc;

    [SerializeField]
    private float swipeDist;
    [SerializeField]
    private float initSwipeDist;
    [SerializeField]
    private float swipeDistInc;


    [SerializeField]
    private int nextLevelXP;
    [SerializeField]
    private int firstLevelXP;

    public bool levelprints = true;

    private PlayerSwiper swiper;
    private GameObject player;
    private TrailCollapser collapser;
    private XPBarController barController;

    private bool levelUpLooping = false;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;        
        swiper = player.GetComponent<PlayerSwiper>();
        collapser = player.GetComponent<TrailCollapser>();
        barController = GameObject.FindGameObjectWithTag("XPBar").GetComponent<XPBarController>();
        nextLevelXP = firstLevelXP;
        barController.maxXpPoints = nextLevelXP;
        maxTrails = initMaxTrails;
        swipeDist = initSwipeDist;
        regenTime = initRegenTime;
        swipeTime = initSwipeTime;
        collapser.setMaxTrails(maxTrails);
        swiper.setSwipeTime(swipeTime);
        swiper.setSwipeLength(swipeDist);
    }

/*
    private void levelUpCallback(){

        if(levelprints)
            print("level up cb was run!");
        if(xp >= nextLevelXP){
            if(levelprints)
                print(string.Format("xp value of {0} found to be greater than next level req of {1}, leveling up again", xp, nextLevelXP));
            nextLevelXP = getNextLevelXP(nextLevelXP);
            barController.maxXpPoints = nextLevelXP;
            levelUp();
        }else{
            barController.setXPDisplayed(xp, nextLevelXP);
            levelUpLooping = false;
        }

    }
    */
    private int getNextLevelXP(int currentLevelXP){
        //here in case we want to make the growth function more complicated
        if(currentLevelXP <= 16){
            return currentLevelXP * 2;
        }else{
            return currentLevelXP;
        }
    }
    private void levelUp(){
        //the arg to getNextLevel is eval'd at current
        //level's xp requirement
        xp -= nextLevelXP;
        if(levelprints)
            print(string.Format("xp that was required for level up was {0}, new xp value is {1}",nextLevelXP,xp));
        nextLevelXP = getNextLevelXP(nextLevelXP);
        barController.maxXpPoints = nextLevelXP;
        //here in case we want this game mechanic
        regenTime += regenTimeInc;
        level += 1;
        updateMaxTrails();
        /*
        if(levelprints)
            print(string.Format("level is now {0}, next requires {1}", level,nextLevelXP));
        //register a callback in case we need to level up multiple times
        levelUpLooping = true;
        barController.levelUp(levelUpDelInst);
        */
        
    }

    private void updateMaxTrails(){
        //here in case we want to make this more complex
        maxTrails += maxTrailsInc;
        collapser.setMaxTrails(maxTrails);
    }

/*
    private void updateSwipeTime(){
        //here in case we want to make this more complex
        swipeTime -= swipeTimeDec;
        swiper.setSwipeTime(swipeTime);
        if(levelprints)
            print("new swipe time is " + swipeTime);
    }

    private void updateSwipeDist(){
        //here in case we want to make this more complex
        swipeDist += swipeDistInc;
        swiper.setSwipeLength(swipeDist);
        if(levelprints)
            print("new swipe dist is " + swipeDist);
    }
    */

    public void trailCollapsed(){
        barController.barAppear();
    }

    public void giveFlitXP(){
        xp += flitXPVal;
        if(!levelUpLooping)
            StartCoroutine("animateXPCorout");
        //TODO: test another call while already= animatingj:w


        
    }

    public int getXP(){
        return xp;
    }

    IEnumerator animateXPCorout()
    {
        levelUpLooping = true;
        while(xp >= nextLevelXP){
            //set xp to max, starting the process of filling the bar
            barController.setXPWithTween(nextLevelXP);
            //delay how long the xp takes to get there
            yield return new WaitForSeconds(barController.XPDisplayTeaseTime);
            barController.levelUpAnimate();
            levelUp();
            yield return new WaitForSeconds(barController.XPDisplayLevelUpTime);
            barController.setXPNoTween(0);
        }
        barController.setXPWithTween(xp);
        levelUpLooping = false;
        yield return null;
        //xp and nextLevel xp will be modified before the following line

    }

/*
    private IEnumerator updateXP(){
        if(xp >= nextLevelXP){
            yield return new WaitWhile(() => levelUpLooping);
            levelUp();
        }else{
            barController.setXPDisplayed(xp, nextLevelXP);
        }
    }
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    [SerializeField]
    public float xp = 0; //public for tutorial manager

    [SerializeField]
    private int level = 0;
    [SerializeField]
    private int maxTrails;
    [SerializeField]
    private int maxTrailsInc;


    [SerializeField]
    private float swipeTime;
    [SerializeField]
    private float initSwipeTime;

    [SerializeField]
    public float regenTime;
    [SerializeField]
    private float initRegenTime;

    [SerializeField]
    private float swipeDist;
    [SerializeField]
    private float initSwipeDist;


    [SerializeField]
    public float nextLevelXP; //public for tutorial manager
    [SerializeField]
    private int firstLevelXP;

    public bool levelprints = true;

    private PlayerSwiper swiper;
    private GameObject player;
    private TrailCollapser collapser;
    private XPBarController barController;
    private LevelUpNotes levelNotes;
    private DifficultyConstants constants;

    private bool levelUpLooping = false;
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;        
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        constants = manager.GetComponent<DifficultyConstants>();
        swiper = player.GetComponent<PlayerSwiper>();
        collapser = player.GetComponent<TrailCollapser>();
        barController = GameObject.FindGameObjectWithTag("XPBar").GetComponent<XPBarController>();
        levelNotes = GameObject.FindGameObjectWithTag("AudioManager").GetComponentInChildren<LevelUpNotes>();
        nextLevelXP = constants.firstLevelXp;
        barController.maxXpPoints = nextLevelXP;
        maxTrails = constants.initialTrailCount;
        swipeDist = initSwipeDist;
        regenTime = constants.regenTime;
        swipeTime = initSwipeTime;
        collapser.setMaxTrails(maxTrails);
        swiper.setSwipeTime(swipeTime);
        swiper.setSwipeLength(swipeDist);
    }

    private float getNextLevelXP(float currentLevelXP){
        if(currentLevelXP <= constants.maxXpPerLevel){
            return currentLevelXP * constants.xpIncreaseFactor;
        }else{
            return currentLevelXP;
        }
    }
    private void levelUp(){
        //the arg to getNextLevel is eval'd at current
        //level's xp requirement
        xp -= nextLevelXP;
        levelNotes.playLevelUpChords();
        if(levelprints)
            print(string.Format("xp that was required for level up was {0}, new xp value is {1}",nextLevelXP,xp));

        
        nextLevelXP = getNextLevelXP(nextLevelXP);
        barController.maxXpPoints = nextLevelXP;
        level += 1;
        updateMaxTrails();
        
    }

    private void updateMaxTrails(){
        //here in case we want to make this more complex
        maxTrails += maxTrailsInc;
        collapser.setMaxTrails(maxTrails);
    }

    public void trailCollapsed(){
        barController.barAppear();
    }

    public void giveFlitXP(){
        xp += constants.xpPerKill;
        if(!levelUpLooping)
            StartCoroutine("animateXPCorout");

        
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

    }

}

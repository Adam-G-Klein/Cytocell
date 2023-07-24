using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<FlitController> flits = new List<FlitController>();
    public GameObject[] walls;

    public Vector2[] purgingZone = null;
    private bool purgeCalled, purgeReset = false;

    private VisualManager ui;
    private GameObject player;
    private PlayerManager playerManager;
    private XPManager xpManager;
    private ObjectRecycler recycler;
    private SwipeNotes sNotes;
    private DeathNotes dNotes;
    public int flitCount;
    public bool splittingEnabled = true;
    private int purgesReported = 0;
    private float camDistance = -10;
    public bool menuMode = false;

    public int nextTrailId = 0;
    public int score = 0;
    public int spawnThreshold = 0;
    public float spawnDistFromWall = 5;
    public bool gamePaused = false;
    private DifficultyConstants constants;

    public int currentPurgeKillCount = 0;
    public ResolutionSetter res; 

    // Use this for initialization
    void Start()
    {
        Debug.Log("GameManager Start");
        LeanTween.init(1600);
        score = 0;
        constants = GetComponent<DifficultyConstants>();
        /* //for testing js integration
        res = GameObject.FindGameObjectWithTag("ResolutionControls").GetComponent<ResolutionSetter>();
        res.setResolution(42);
        */

        if (!menuMode)
        {
            ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<VisualManager>();
            player = GameObject.FindGameObjectWithTag("Player");
            xpManager = player.GetComponent<XPManager>();
            recycler = GetComponent<ObjectRecycler>();
            walls = GameObject.FindGameObjectsWithTag("Wall");
            playerManager = player.GetComponent<PlayerManager>();

            sNotes = GameObject.Find("Audio").GetComponentInChildren<SwipeNotes>();
            dNotes = GameObject.Find("Audio").GetComponentInChildren<DeathNotes>();

            spawnFlits();

        }
    }

    // Update is called once per frame
    void Update()
    {

        //dumb convoluted way of doing a state machine for resetting the purge
        /*if (purgeReset)
        {
            purgingZone = null;
            purgeReset = purgeCalled = false;

        }
        if (purgeCalled) purgeReset = true;
        */
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
        }

        //only enable splitting if we're less than the max flits 
        splittingEnabled = flitCount <= constants.MaxEnemies;




    }


    public void trailCreated()
    {
        nextTrailId += 1;
    }
    public void flitCreated(FlitController flitControl)
    {
        //print("gamemanager adding " + flitControl.gameObject.name);
        flits.Add(flitControl);
        flitCount += 1;

    }

    public void flitKilled(FlitController flitControl)
    {
        flitCount -= 1;
        flits.Remove(flitControl);
        if (flitCount <= constants.SpawnMoreThreshold)
        {
            spawnFlits();
        }
        if(flitControl.noScoreOrXPonDeath) return;

        score += 1;
        ui.updateScore(score);
    }
    public void spawnFlits()
    {
        /*
        GameObject newflit2 = recycler.RecycleFlit(new Vector3(1f, 2f, 0), transform.rotation);
        GameObject newflit3 = recycler.RecycleFlit(new Vector3(-1f, -2f, 0), transform.rotation);
        */
        print("SPAWNING MORE ENEMIES");
        Vector3 spawnWallPos;
        int i = 0;
        while (i < constants.AmountToSpawnWhenCleared)
        {
            spawnWallPos = walls[Random.Range(0, 7)].transform.position;
            Vector3 newFlitPos = getSpawnPos(spawnWallPos);
            GameObject newflit1 = recycler.RecycleFlit(newFlitPos, transform.rotation);
            i += 1;
        }
    }
    private Vector3 getSpawnPos(Vector3 wallPos)
    {
        return Vector3.Lerp(wallPos, Random.insideUnitCircle, spawnDistFromWall);
    }
    public void giveFlitXP()
    {
        xpManager.giveFlitXP();
    }

    public void purgeKillZone(Vector2[] killZone)//GameObject killZone)
    {
        currentPurgeKillCount = 0;
        bool flitKilled = false;
        Debug.Log("PurgeKillZone called");
        sNotes.nextChord();
        dNotes.currNote = 0;
        Debug.Log("Got past swipe notes");


        foreach (FlitController flitControl in flits)
        {
            //will set toBePurged on each flit inside killzone
            flitKilled = flitControl.checkPurged(killZone);
            //if this flit was killed and we havent indicated
            // the player got something yet, flip the bool
            if (flitKilled) currentPurgeKillCount += 1;
        }
        if (currentPurgeKillCount > 0)
        {
            playerManager.startRegen(currentPurgeKillCount);
        }
        //need to do this loop twice because we can't purge flits during 
        //list iteration: remove can happen anywhere in the list,
        //so C# is gonna lose track of where we were iterating
        /*         foreach(FlitController flitControl in flits) {
                    if(flitControl.isToBePurged()){
                        flitControl.purgeAtEndOfFrame();
                        purged += 1;
                    }
                }*/


        /* 

        purgingZone = killZone;
        purgeCalled = true;
        purgeReset = false;
        */
    }

    public float getCamDistance() { return camDistance; }

    public void playerKilled()
    {
        int hScore = PlayerPrefs.GetInt("Highscore"+constants.SceneName, -1);
        if (hScore == -1 || score > hScore)
        {
            ui.newHighScore(score);
            PlayerPrefs.SetInt("Highscore"+constants.SceneName, score);
        }

        ui.displayDeathMessage();
    }

}

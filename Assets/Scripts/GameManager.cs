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
        LeanTween.init(1600);
        score = 0;
        constants = GetComponent<DifficultyConstants>();

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
        //TODO: event bus so that we don't have to poll for this
        splittingEnabled = flitCount <= constants.MaxEnemies;

    }


    public void trailCreated()
    {
        nextTrailId += 1;
    }
    public void flitCreated(FlitController flitControl)
    {
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

    public bool soundEnabled()
    {
        return PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
    }

    public void setSoundEnabled(bool enabled)
    {
        PlayerPrefs.SetInt("SoundEnabled", enabled ? 1 : 0);
    }

}

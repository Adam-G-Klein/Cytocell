using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<FlitController> flits = new List<FlitController>();

    public Vector2[] purgingZone = null;
    private bool purgeCalled, purgeReset = false;

    private VisualManager ui;
    private GameObject player;
    private XPManager xpManager;
    private ObjectRecycler recycler;
    public int flitCount;
    public int maxFlitCount;
    public bool splittingEnabled = true;
    private int purgesReported = 0;
    private float camDistance = -10;
    public bool menuMode = false;
 
    public int nextTrailId = 0;
	// Use this for initialization
	void Start () {
        LeanTween.init(1600);
        ui = gameObject.GetComponent<VisualManager>();
        if(!menuMode){
            player = GameObject.FindGameObjectWithTag("Player");
            xpManager = player.GetComponent<XPManager>();
            recycler = GetComponent<ObjectRecycler>();

        }
	}
	
	// Update is called once per frame
	void Update () {

        //dumb convoluted way of doing a state machine for resetting the purge
        /*if (purgeReset)
        {
            purgingZone = null;
            purgeReset = purgeCalled = false;

        }
        if (purgeCalled) purgeReset = true;
        */

        //only enable splitting if we're less than the max flits 
        splittingEnabled = flitCount <= maxFlitCount;
       
        
        
        
	}


    public void trailCreated(){
        nextTrailId += 1;
    }
    public void flitCreated(FlitController flitControl){
        //print("gamemanager adding " + flitControl.gameObject.name);
        flits.Add(flitControl);
        flitCount+=1;

    }

    public void flitKilled(FlitController flitControl){
        flitCount -= 1;
        flits.Remove(flitControl);
        if(flitCount <= 0){
            spawnFlits();
        }
    }
    public void spawnFlits(){
        //this will be improved
        print("SPAWNING MORE ENEMIES");
        GameObject newflit1 = recycler.RecycleFlit(new Vector3(-0.7f, -0.2f, 0), transform.rotation);
        GameObject newflit2 = recycler.RecycleFlit(new Vector3(1f, 2f, 0), transform.rotation);
        GameObject newflit3 = recycler.RecycleFlit(new Vector3(-1f, -2f, 0), transform.rotation);
        
        
    }
    public void giveFlitXP(){
        xpManager.giveFlitXP();
    }

    public void purgeKillZone(Vector2[] killZone)//GameObject killZone)
    {
        foreach(FlitController flitControl in flits) {
            //will set toBePurged on each flit inside killzone
            flitControl.checkPurged(killZone);
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

    public void playerKilled(){
        ui.displayDeathMessage();
    }

/*
    IEnumerator waitToRevokePurgePermission(int purged){

        //problem is probably here
        while(purged > purgesReported){yield return null;}
        purgesReported = 0;
        purgePermission = false;



    }
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCollapser : MonoBehaviour {
    /*handles the large-scale manipulation of all trails 
     *named collapser because that is the main large-scale functionality
     *that it performs
     *also removes trails when one more than maxTrails is created*/
    public List<GameObject> activeTrails = new List<GameObject>();
    public List<TrailController> collapsingTrails = new List<TrailController>();
    public List<TrailController> edgeTrails = new List<TrailController>();
    private List<Vector2> lastKillzonePoints = new List<Vector2>();


    private GameManager gameManager;
    public bool collapseTriggered = false;
    public GameObject killZone;
    private Transform player;
    private bool firstTrailFound = false;
    private int maxTrails;
    private PlayerSwiper swiper;
    private XPManager xpManager;
    private KillzoneAnimation killAnim;
    // Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        killZone = GameObject.FindGameObjectWithTag("KillZone");
        swiper = player.GetComponent<PlayerSwiper>();
        xpManager = player.GetComponent<XPManager>();
        killAnim = GetComponent<KillzoneAnimation>();
        
	}
	
    public void triggerCollapse(){
        collapseTriggered = true; //only for the tutorial at this point, don't need to reset
        TrailController firstTrail = null;
        for(int i = activeTrails.Count - 1; i >= 0; i--){
            TrailController trailController = activeTrails[i].GetComponent<TrailController>();
            if (trailController.trailCrossed){
                firstTrail = trailController;
            }
            if (firstTrail != null){
                collapsingTrails.Add(trailController);
            }
            else
                edgeTrails.Add(trailController);
        }
        
        if(killAnim) killAnim.animate(collapsingTrails);
        activeTrails.Clear();
        print("collapsing trails: " + collapsingTrails.Count);
        collapseTrails(collapsingTrails, firstTrail);
        disableTrails(edgeTrails);
        gameManager.nextTrailId = 0;
        xpManager.trailCollapsed();
        edgeTrails.Clear();


    }

    public int getMaxTrails(){
        // just for the tutorial
        return maxTrails;
    }

    public void setMaxTrails(int newMaxTrails){
        //called by xpManager
        maxTrails = newMaxTrails;
    }
    public void trailCreated(GameObject trail){
        
        TrailController trailController = trail.GetComponent<TrailController>();
        if(!swiper.swipeEnabled){
            // handle edge case where trails are disabled mid-swipe
            trailController.die();
            return;
        }

        activeTrails.Add(trail);
        // TODO: optimize to not GetComp at runtime
        if(activeTrails.Count == maxTrails){
            activeTrails[0].GetComponent<TrailController>().nextToDie();
        }
        if(activeTrails.Count > maxTrails){
            TrailController removedTrail = activeTrails[0].GetComponent<TrailController>();
            removedTrail.die();
            activeTrails.RemoveAt(0);
            activeTrails[0].GetComponent<TrailController>().nextToDie();
        }

    }
    public void killAllTrails(){
        //used when player receives knockback from enemies or walls
        int cnt = 0;
        foreach(GameObject trail in activeTrails){
            trail.GetComponent<TrailController>().die();
            cnt+=1;
        }
        activeTrails.Clear();
    }

    private void printObjList(List<GameObject> objs){
        int i;
        for(i = 0; i < objs.Count; i++)
            print(string.Format("\tobj{0} name: {1}", i, objs[i].name));
    }

    private void printTrailList(List<TrailController> objs){
        int i;
        for(i = 0; i < objs.Count; i++)
            print(string.Format("\tobj{0} name: {1}", i, objs[i].gameObject.name));
    }



    private void collapseTrails(List<TrailController> collTrails, TrailController firstTrail)
    {
        Vector2[] killZonePoints = getKillZonePoints(collTrails, firstTrail);
        foreach(TrailController trail in collTrails)
        {
            trail.collapse();
        }
        gameManager.purgeKillZone(killZonePoints);
        collapsingTrails.Clear();

    }

    void OnDrawGizmos()
    {
        if (lastKillzonePoints.Count > 0)
        {
            Gizmos.color = Color.red;
            for(int i = 0; i < lastKillzonePoints.Count; i++)
            {
                Gizmos.DrawLine(lastKillzonePoints[i], lastKillzonePoints[(i + 1) % lastKillzonePoints.Count]);
            }
        }
    }
    
    private Vector2[] getKillZonePoints(List<TrailController> trails, TrailController firstTrail)
    {
        print("getKillZonePoints called with trails: " + trails.Count);
        lastKillzonePoints.Clear();
        List<Vector2> killZonePoints = new List<Vector2>();
        
        foreach (TrailController trail in trails)
        {
            //end points are stored in a 2 item array
            foreach (Vector2 endpoint in trail.endpoints)
            {

                killZonePoints.Add(endpoint);
                //print(string.Format("added kill point {0}", endpoint));
            }

        }
        /*
        TrailController lastTrail = trails[trails.Count - 1];
        killZonePoints.Add(lastTrail.endpoints[0]); 
        killZonePoints.Add(firstTrail.endpoints[1]); //add the first point back
        */
        lastKillzonePoints = killZonePoints;

        return killZonePoints.ToArray();
    }

    private Vector2[] getLocalPoints(Transform parent, Vector2[] globalPoints)
    {
        List<Vector2> localPoints = new List<Vector2>();
        foreach (Vector2 point in globalPoints)
        {
            localPoints.Add(parent.InverseTransformPoint(point));
        }
        return localPoints.ToArray();

    }

    private void disableTrails(List<TrailController> trails)
    {
        int enqCount = 0;
        foreach (TrailController trail in trails)
        {
            enqCount += 1;
            
            //recylcer.trails.Enqueue(obj);
            trail.die();
        }

        //print("enq count" + enqCount);
    }
}

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


    private GameManager gameManager;
    private ObjectRecycler recylcer;
    public bool collapseTriggered = false;
    public GameObject killZone;
    private Transform player;
    private bool firstTrailFound = false;
    private int maxTrails;
    private PlayerSwiper swiper;
    private XPManager xpManager;
    // Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        killZone = GameObject.FindGameObjectWithTag("KillZone");
        recylcer = gameManager.GetComponent<ObjectRecycler>();
        swiper = player.GetComponent<PlayerSwiper>();
        xpManager = player.GetComponent<XPManager>();
        
	}
	
	// Update is called once per frame
	void Update () {
        //print("trails: " + activeTrails.Count);

	}
    public void triggerCollapse(){
        bool firstTrailFound = false;
        foreach(GameObject trail in activeTrails)
        {
            if (trail.GetComponent<TrailController>().trailCrossed){
                firstTrailFound = true;
            }
            if (firstTrailFound){
                collapsingTrails.Add(trail.GetComponent<TrailController>());
            }
            else
                edgeTrails.Add(trail.GetComponent<TrailController>());
        }
        activeTrails.Clear();
        collapseTrails(collapsingTrails);
        disableTrails(edgeTrails);
        gameManager.nextTrailId = 0;
        xpManager.trailCollapsed();

    }

    public void setMaxTrails(int newMaxTrails){
        //called by xpManager
        maxTrails = newMaxTrails;
    }
    public void trailCreated(GameObject trail){
        


        if(!swiper.swipeEnabled){
            trail.die();
            return;
        }


        activeTrails.Add(trail);
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
        //print("cleared " + cnt + " trails");

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



    private void collapseTrails(List<TrailController> collTrails)
    {
        Vector2[] killZonePoints = getKillZonePoints(collTrails);
        //print("trail collapsed!");
        //GameObject zone = Instantiate(killZone, killZonePoints[0], collTrails[0].transform.rotation);
        /*Transform killTran = killZone.transform;
        killTran.position = killZonePoints[0];
        killTran.rotation = collTrails[0].transform.rotation;
        EdgeCollider2D coll = killZone.GetComponent<EdgeCollider2D>();

        coll.points = getLocalPoints(killZone.transform, killZonePoints);*/

        disableTrails(collTrails);
        //manager.purgeKillZone(killZone);
        gameManager.purgeKillZone(killZonePoints);
        collapsingTrails.Clear();

    }

    private Vector2[] getKillZonePoints(List<TrailController> trails)
    {
        List<Vector2> killZonePoints = new List<Vector2>();
        killZonePoints.Add(trails[0].endpoints[1]);//add the second point of the first trail
        
        foreach (TrailController trail in trails.GetRange(1,trails.Count-1))
        {
            
            //end points are stored in a 2 item array
            foreach (Vector2 endpoint in trail.endpoints)
            {
                killZonePoints.Add(endpoint);
                //print(string.Format("added kill point {0}", endpoint));
            }

        }
        killZonePoints.Add(killZonePoints[0]); //add the first point back

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
            
            GameObject obj = trail.gameObject;
            //recylcer.trails.Enqueue(obj);
            trail.die();

        }

        //print("enq count" + enqCount);
    }
}

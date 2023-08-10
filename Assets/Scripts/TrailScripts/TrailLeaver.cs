using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailLeaver : MonoBehaviour {

    private Transform player;
    private PlayerSwiper plSwipe;
    private PlayerMover mover;
    public GameObject trail;
    
    private SwipeController swiper;
    private TrailCollapser collapser;

    private Transform newTrail;
    private IEnumerator corout;
    private ObjectRecycler recycler;
    private XPManager xpManager;
    public bool trailPlacing = false;
    private GameObject managerObj;
    private GameManager manager;
    [SerializeField]
    private string trailName;
    public float psInstDelay;
    public GameObject swipePS;
    public Vector2 swipePSOffset;

	// Use this for initialization
	void Start () {
        player = transform;
        xpManager = player.GetComponent<XPManager>();
        mover = player.GetComponent<PlayerMover>();
        swiper = GetComponent<SwipeController>();
        plSwipe = GetComponent<PlayerSwiper>();
        collapser = GetComponent<TrailCollapser>();
        managerObj = GameObject.FindGameObjectWithTag("GameManager");  
        recycler = managerObj.GetComponent<ObjectRecycler>();
        manager = managerObj.GetComponent<GameManager>();

	}
	
	// Update is called once per frame
	void Update () {

        if (plSwipe.plSwiped)
        {
            //so we don't call newTrail before a trail has been placed, 
            //or after they're destroyed
            Invoke("instantiatePS", psInstDelay);
            if (collapser.activeTrails.Count != 0) 
                //this sets the last trail we left to know it's been fully placed
                //we can only know this is true on the next swipe
                //doing otherwise places unnecessary constraints on the player control system
                newTrail.GetComponent<TrailController>().trailPlaced = true;
            if (trailPlacing)
                StopCoroutine(corout);
            trailPlacing = true;
            newTrail = recycler.RecycleTrail(player.position, player.rotation).transform;
            TrailController newTrailCont = newTrail.GetComponent<TrailController>();
            newTrailCont.collapser = collapser;
            newTrailCont.trailNum = manager.nextTrailId;
            newTrailCont.recycler = recycler;
            newTrail.name = trailName 
                 + manager.nextTrailId;
            manager.trailCreated();
            collapser.trailCreated(newTrail.gameObject);
            corout = TrailPlacer(newTrail);
            StartCoroutine(corout);
            

        }

	}
    private void instantiatePS(){
        GameObject psgo = Instantiate(swipePS,transform, false);
        psgo.transform.localPosition = swipePSOffset;
    }
    public void currentTrailPlaced(){
        if(newTrail != null)
            newTrail.GetComponent<TrailController>().trailPlaced = true;
    }
    IEnumerator TrailPlacer(Transform trail)
    {
        TrailController trailCont = trail.GetComponent<TrailController>();
        Vector2 startPos = player.position;
        Vector3 newAngs, newScale;
        Vector2 newPos;
        // + 90 due to experiment based implementation
        newAngs = new Vector3(0, 0,mover.FindAngle(swiper.lastDir) + 90);
        //this should be interruptable by another trail being formed
        while (LeanTween.isTweening(mover.ltidMov) && !trailCont.isTrailPlaced())
        {
            newPos = (((Vector2)player.position - startPos) / 2) + startPos;
            newScale = new Vector3(Vector2.Distance(startPos, player.position),
                                    trail.localScale.y, trail.localScale.z);
            trail.eulerAngles = newAngs;
            trail.localScale = newScale;

            trail.position = newPos;
            yield return new WaitForEndOfFrame();
        }
        trail.GetComponent<TrailController>().trailPlaced = true;
        trailPlacing = false;
    }
}

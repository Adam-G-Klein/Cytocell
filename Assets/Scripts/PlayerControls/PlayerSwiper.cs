using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Coordinates 
public class PlayerSwiper : MonoBehaviour {

    private float swipeTime = 0.5f, rotTime;
    // Now the ratio of the swipe magnitude to the distance the player will move
    public float swipeMultiplier = 0.001f;
    public float maxSwipeLength = 1f;
    public bool mouseDebug;
    public bool plSwiped = false;
    public bool swipeInterruptable = false;
    private PlayerMover mover;
    public SwipeController swipeCont;
    public Camera cam;
    public Vector2 movDir;
    
    private Rigidbody2D rbody; 
    private Transform player;
    private Vector2 dir,mpWorldPoint;
    private Vector2 movTarg;

    private float targAngle = 0;
    private float z = 10;

    private float prevTarg = 0;
    private float rotT,perc = 0;
    public bool swipeEnabled = true;

    private Vector2 targDir,movTa;
    private Vector2 mp;
    private GameManager manager;
    private bool movSet = false;
    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        swipeCont = GetComponent<SwipeController>();
        mover = GetComponent<PlayerMover>();

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, manager.getCamDistance());
        rbody = GetComponent<Rigidbody2D>();
        swipeEnabled = true;
    }

    // Update is called once per frame
    void Update() {
        #region mouse debug
        if (mouseDebug)
        {
            mp = Input.mousePosition;
            mpWorldPoint = (Vector2)cam.ScreenToWorldPoint(new Vector3(mp.x, mp.y, z));
            print(string.Format("mp: {0} wp: {1} z: {2}", (Vector2)mp, mpWorldPoint, z));
        }
        #endregion
        
        // Use a oneframe boolean to communicate to outher systems.
        // TODO: Actual event bus implementation
        if (plSwiped)
            plSwiped = false;
        if(swipeCont.swiped && !swipeEnabled) {
            print("swipe rejected");
        }
        if (swipeCont.swiped && swipeEnabled)
        {
            //these lines used to be under this conditional because we didn't know if we wanted swipe
            //to be interruptable. If want uniterruptable swipes, restore this line
            //if (!(ltidMov != 0 && LeanTween.isTweening(ltidMov)) || swipeInterruptable)
            print("accepted swipe, swipeInterruptable: " + swipeInterruptable + " swipeEnabled: " + swipeEnabled);
            float swipeLength = Mathf.Clamp(swipeCont.lastDir.magnitude * swipeMultiplier, 0, maxSwipeLength);
            
            mover.movePlayer(swipeCont.lastDir, swipeLength, swipeTime, rotTime);
            //make sure trailleaver does it's stuff after the player is moving
            plSwiped = true;

        }
    }

    public void setSwipeTimeAndDist(float newSwipeTime, float newSwipeDistance){
        swipeTime = newSwipeTime;
        swipeMultiplier = newSwipeDistance; 
    }
    public void setSwipeTime(float newSwipeTime){
        swipeTime = newSwipeTime;
        rotTime = newSwipeTime/4;
    }

    public void setSwipeLength(float newSwipeLen){
        maxSwipeLength = newSwipeLen;
    }
}

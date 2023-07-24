using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwiper : MonoBehaviour {

    private float swipeTime = 0.5f, rotTime;
    public float swipeLength = 1f;
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
        #region Old Coroutine
        /*
        if (swipeCont.swiped)
        {
            StopCoroutine("MoveCoroutine");
            //targDir = (Vector2)cam.ScreenToWorldPoint(new Vector3(swipeCont.direction.x, swipeCont.direction.y, z));
            targ = (swipeCont.direction.normalized * swipeLength) + (Vector2) player.position;
            print(string.Format("targDir: {0} targ: {1}", swipeCont.direction.normalized, targ));
            StartCoroutine("MoveCoroutine",targ);
            
            
        }
        */
        #endregion
        #region mouse debug
        if (mouseDebug)
        {
            mp = Input.mousePosition;
            mpWorldPoint = (Vector2)cam.ScreenToWorldPoint(new Vector3(mp.x, mp.y, z));
            print(string.Format("mp: {0} wp: {1} z: {2}", (Vector2)mp, mpWorldPoint, z));
        }
        #endregion
        //cam.transform.position = new Vector3(transform.position.x, transform.position.y, manager.getCamDistance());
        /*if (ltidMov != 0 && movSet && !LeanTween.isTweening(ltidMov))
        {
            print(string.Format("rot tweening: {0} mov tweening: {1}", LeanTween.isTweening(ltidRot), LeanTween.isTweening(ltidMov)));
            movSet = false;
        }*/
        //print(string.Format("playervel: {0} {1}", rbody.velocity.x, rbody.velocity.y));
        if (plSwiped)
            plSwiped = false;
        if(swipeCont.swiped && !swipeEnabled)
            print("swipe rejected");
        if (swipeCont.swiped && swipeEnabled)
        {
            //these lines used to be under this conditional because we didn't know if we wanted swipe
            //to be interruptable. If want uniterruptable swipes, restore this line
            //if (!(ltidMov != 0 && LeanTween.isTweening(ltidMov)) || swipeInterruptable)
            
            mover.movePlayer(swipeCont.lastDir, swipeLength, swipeTime, rotTime);
            //make sure trailleaver does it's stuff after the player is moving
            plSwiped = true;
/*             movDir = (Vector2)swipeCont.direction;
            LeanTween.cancel(ltidMov);
            //StopCoroutine("RotCoroutine");
                            
            LeanTween.cancel(ltidRot);
            LeanTween.cancel(ltidCam);
            movTarg = (swipeCont.direction.normalized * swipeLength) + (Vector2)player.position;
            //
            ltidMov = LeanTween.move(player.gameObject, movTarg, swipeTime)
                        .setEaseInQuad().setEaseOutSine().id;
            ltidCam = LeanTween.move(cam.gameObject, new Vector3(movTarg.x,movTarg.y, cam.transform.position.z), swipeTime)
                        .setEaseInQuad().setEaseOutSine().id;
            //movSet = true
            targAngle = FindAngle(swipeCont.direction);
            ltidRot = LeanTween.rotate(player.gameObject,
                        new Vector3(player.rotation.x, player.rotation.y, targAngle), rotTime)
                        .setEaseInQuad().setEaseOutSine().id;
                        */
        }
    }

    public void setSwipeTimeAndDist(float newSwipeTime, float newSwipeDistance){
        swipeTime = newSwipeTime;
        swipeLength = newSwipeDistance; 
    }
    public void setSwipeTime(float newSwipeTime){
        swipeTime = newSwipeTime;
        rotTime = newSwipeTime/4;
    }

    public void setSwipeLength(float newSwipeLen){
        swipeLength = newSwipeLen;
    }


    #region Coroutines
    IEnumerator MoveCoroutine(Vector2 target)
    {
        float t = 0.0f;
        float perc = 0.0f;
        Vector2 start = transform.position;

        Vector2 end = target;

        print(string.Format("worldTarg: {0} st: {1} en {2}", target, start, end));
        while (t < swipeTime)
        {
            t += Time.deltaTime;
            perc = t / swipeTime;
            perc = perc * perc * perc * (perc * (6f * perc - 15f) + 10f);
            transform.position = Vector2.Lerp(start, end, perc);
            //print(string.Format("lerping, t: {0}", 1/t));
            yield return null;
        }
    }

    IEnumerator RotCoroutine(float target)
    {
        float t = 0.0f;
        float perc = 0.0f;
        float ang;
        float start = transform.eulerAngles.z;
        float end = target;

        print(string.Format("targ: {0} st: {1} en {2}", target, start, end));
        while (t < rotTime)
        {
            t += Time.deltaTime;
            perc = t / rotTime;
            //perc = perc * perc * perc * (perc * (6f * perc - 15f) + 10f);
            ang = Mathf.LerpAngle(start, end, perc);
            transform.eulerAngles = new Vector3(0,0,ang);
            //print(string.Format("lerping, t: {0}", 1/t));
            yield return null;
        }
    }


    #endregion



}

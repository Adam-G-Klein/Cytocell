using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlitController : MonoBehaviour
{

    private GameManager manager;
    private ObjectRecycler recycler;
    private Collider2D coll;
    private MotionScaling scaler;
    private int ltidMov = 0;
    [SerializeField]
    private float recycleWaitTime;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    public float flitDistance;
    [SerializeField]
    private float distError;
    [SerializeField]
    private int randMovOdds = 100;
    [SerializeField]
    private float randMovDistMax;
    [SerializeField]
    private float randMovSpeedRatio;
    // the minimum personal radius a flit wants 
    // may need to change with enemy density
    public float pSpace;
    [SerializeField]
    private float pSpaceSpeedRatio;
    [SerializeField]
    private float pSpaceMovDistMax;
    [SerializeField]
    private float pSpaceDistWeight;
    [SerializeField]
    private float minPSpaceDistWeight;
    [SerializeField]
    private float minSplitTimer;
    [SerializeField]
    private float maxSplitTimer;
    [SerializeField]
    private float splitTimer;
    [SerializeField]
    private float splitDistance;
    [SerializeField]
    private float splitSpeedRatio;
    [SerializeField]

    private float trailAvoidanceWeight = 1;
    [SerializeField]

    private float wallAvoidanceWeight = 1;
    [SerializeField]

    private float flitAvoidanceWeight = 1;
    [SerializeField]
    private GameObject selfPrefab; 

    private Collider2D[] pSpaceHits = new Collider2D[20];

    private String flitName = "Flit";
    [SerializeField]
    private float knockBack;
    [SerializeField]
    private float knockMovTime;
    [SerializeField]
    private float knockRotTime;
    public bool toBePurged = false;

 
    private float ltidScale = 0;
    private float ltidAlpha = 0;
    private float ltidColor = 0;
    
    private JellyDeathAnimation.deathDelegate deathDelegate;

    //states occur inside coroutines
    //but states have priority that overrides others
    public enum FlitState
    {
        dying, splitting, spacing, randmoving, idle
    }
    [SerializeField]
    private FlitState state = FlitState.idle;
    private FlitController fController;
    private JellyDeathAnimation deathAnimation;

    private bool startHasRun = false;
    // Use this for initialization
    void Start()
    {
        
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //manager.flitCreated(this);
        
        recycler = manager.GetComponent<ObjectRecycler>();
        coll = GetComponent<Collider2D>();
        //this is just for dumping this flit
        fController = GetComponent<FlitController>();
        splitTimer = getSplitTime(minSplitTimer,maxSplitTimer);
        scaler = GetComponent<MotionScaling>();
        deathAnimation = GetComponent<JellyDeathAnimation>();
        ltidMov = 0;
        deathDelegate = deathCallback;
        //if(!startHasRun) manager.flitCreated(fController);
        startHasRun = true;
        //tween testing
        /*Vector2 movTarg = transform.position + new Vector3(3, 3, 0);
        print("movTarg: " + movTarg.ToString());
        float speed = maxSpeed/3;
        ltidMov = LeanTween.move(gameObject, movTarg,
                         flitDistance / speed)
                           .setEaseInQuad().setEaseOutSine().id;*/
    }

    // Update is called once per frame
    void Update()
    {
        checkStateOverrides();
        updateTimers();

    }
    /*
    void LateUpdate(){
        if(toBePurged) purge();
        toBePurged = false;
    }
    */
    public void respawnReset(){
        state = FlitState.idle;
        deathAnimation.resetForRespawn();
    }

    private void checkStateOverrides()
    {
        //coroutines will return state to idle when finished
        switch (state)
        {
            case FlitState.dying:
                break;
            case FlitState.spacing:
                //spreader will set state to spreading if overriden
                //spreader is only override
                break;
            case FlitState.randmoving:
                //checkPSpace(); //sets state to spacing if not properly spaced   
                //not using bool return from checkPSpace
                if (checkSplitTime())//if true, not time to split
                {
                    checkPSpace();
                }
                break;
            case FlitState.idle:
                if (checkSplitTime())//if true, not time to split
                {
                    if (checkPSpace())
                    {
                        // in here, we're properly spaced
                        randOdds(); //sets state to randMoving if odds good
                    }
                }
                
                    
                break;
            default:
                break;
        }
    }

    public float[] getKnockBackInfo(){
        /*get info on knockback
        0: knockBackDist
        1: knockMovTime
        2: knockRotTime*/
        return new float[]{knockBack,knockMovTime,knockRotTime};
    }
    

    private void randOdds()
    {
        //print("randodds called");
        int randInt = UnityEngine.Random.Range(0, (int) randMovOdds);
        if (randInt == 0)
        {
            //StopAllCoroutines();
            StopCoroutine("moveTo");
            Vector2 randPoint = (UnityEngine.Random.insideUnitCircle * randMovDistMax) + new Vector2(transform.position.x, transform.position.y);

            StartCoroutine("moveTo", new Vector3(randPoint.x, randPoint.y, maxSpeed * randMovSpeedRatio));
            state = FlitState.randmoving;
        }
    }
    private bool checkPSpace()
    {
        
        coll.enabled = false;
        int res = Physics2D.OverlapCircleNonAlloc(transform.position, pSpace, pSpaceHits);
        coll.enabled = true;
        Vector2 moveVector = new Vector2();
        List<String> hitObjs = new List<string>();
        if (res > 0)
        {
            
            hitObjs = getObjectTypes(pSpaceHits, res);
            if (state == FlitState.idle || hitObjs.Contains("Trail")
                 || hitObjs.Contains("Wall"))
            {
                moveVector = getMoveVector(pSpaceHits, res);
                //print("obj " + gameObject.name + " move vec: " + moveVector.ToString());
                state = FlitState.spacing;
                StopCoroutine("moveTo"); 
                StartCoroutine("moveTo",
                    new Vector3(moveVector.x + transform.position.x,
                        moveVector.y + transform.position.y, maxSpeed * pSpaceSpeedRatio));

                Debug.DrawLine(transform.position,
                     transform.position + new Vector3(moveVector.x, moveVector.y, transform.position.z),
                     Color.blue,
                     2);
            }

 
            return false;
        }
        
        return true;


    }

    private float getSplitTime(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);

    }

    private void updateTimers()
    {
        splitTimer -= Time.deltaTime;
        
    }

    private bool checkSplitTime()
    {
        
        if (splitTimer <= 0) // && state == FlitState.idle)
        {
            if(manager.splittingEnabled){
                StopCoroutine("moveTo");
                StartCoroutine("split"); //state will be set back and counter reset by coroutine
                state = FlitState.splitting;
                return false;
            }
            else{
                splitTimer = getSplitTime(minSplitTimer,maxSplitTimer);
            }
        }
        return true;
    }

    private Vector2 getMoveVector(Collider2D[] hits, int hitNum)
    {
        Vector2 res = new Vector2();
        
        //bool trailFound = false;
        for(int i = 0; i < hitNum; i ++)
        {
            if (hits[i].tag == "Trail") //if so, add endpoint difs
            {
          //      trailFound = true;
                foreach(Vector2 endpoint in hits[i].GetComponent<TrailController>().endpoints)
                {
                    res += getAvoidanceVector(endpoint, hitNum, trailAvoidanceWeight);
                }
            }else if(hits[i].tag == "Wall"){

                res += getAvoidanceVector(hits[i].transform.position, hitNum, wallAvoidanceWeight);
            }
            else
            {
                res += getAvoidanceVector(hits[i].transform.position, hitNum, flitAvoidanceWeight);
                //print(String.Format("added flit at {0} to avoidance\nmove vector: {1}",hits[i].transform.position.ToString(),res.ToString() ));

            }
        }
        Array.Clear(hits, 0, hitNum);
        return res * pSpaceMovDistMax;

    }


    private Vector2 getAvoidanceVector(Vector2 avoidPos, int numAvoiding, float avoidanceWeight)
    {
        float distance = Vector2.Distance(avoidPos, transform.position);
        //dist ratio gets closer to 1 if closer to flit obj
        float distRatio = 1 - (distance / pSpace); 
        //make sure the ration isn't so small the flit gets into deadlock
        distRatio = distRatio < minPSpaceDistWeight ? minPSpaceDistWeight : distRatio;
        // float distWeight = distRatio - (distRatio * (1 - pSpaceDistWeight));
        Vector2 hitDif = (avoidPos - (Vector2)transform.position).normalized;
        Vector2 addVec = (new Vector2(-hitDif.x, -hitDif.y));
        
        Vector2 res = (((addVec * avoidanceWeight) / numAvoiding) * distRatio);
         
        // print(String.Format("name: {0} distance: {1} distRatio: {2} hitDif: {3} addVec: {4} \nnumAvoid: {5} weight: {6} res: {7} ",
            // gameObject.name, distance, distRatio, hitDif, addVec,numAvoiding, avoidanceWeight, res));
        return res; 

    }
    private List<String> getObjectTypes(Collider2D[] hits, int hitNum)
    {
        List<String> res = new List<string>();

        for(int i = 0; i < hitNum; i++)
        {
            if (!res.Contains(hits[i].tag))
            {
                res.Add(hits[i].tag);
            }
        }
        return res;

    }

    
    public bool checkPurged(Vector2[] killzone)
    {

        if (UtilTools.InsidePolygon(killzone, (Vector2)transform.position))//coll.bounds.Intersects(manager.purgingZone.bounds))
        {
            purge();
            return true;
            //StartCoroutine("purgeAtEndOfFrame");
        }
        else
        {
            return false;
        }

    }
    public bool isToBePurged(){return toBePurged;}

    public void startSplitMovement(Vector2 newPos)
    {
        
        StopCoroutine("splitMov");
        state = FlitState.splitting;
        StartCoroutine("splitMov", new Vector2(newPos.x, newPos.y));

    }

    public void setState(FlitState newState) { state = newState; }


    public void setDestination(FlitState newState, Vector2 newPos, float speedRatio)
    {
        //coroutine waits for forbidden states to finish then moves to newPos at speedRatio
        StartCoroutine("setDest", new Vector4((float)newState, newPos.x, newPos.y, speedRatio));

    }
    
    private IEnumerator setDest(Vector4 args) //x is newState, y and z are destination, w is speedRatio
    {
       
        while (state == FlitState.splitting) yield return null;
        StopCoroutine("moveTo");
        StartCoroutine("moveTo", new Vector3(args.y, args.z, maxSpeed*args.w));
        state = (FlitState) args.x;
    }

    private IEnumerator split()
    {
        //state will be set back and counter reset by coroutine
        while (ltidMov != 0 && LeanTween.isTweening(ltidMov)) {
            yield return null; }
        if(manager.splittingEnabled){
            Vector2 myMovVect = (Vector2)(UnityEngine.Random.onUnitSphere * splitDistance);
            Vector2 otherMovVect = new Vector2(-myMovVect.x, -myMovVect.y);
            Vector2 myNewPos = myMovVect + (Vector2)transform.position;
            Vector2 otherNewPos = otherMovVect + (Vector2)transform.position;

            FlitController newFlit = recycler.RecycleFlit(transform.position,transform.rotation)
                    .GetComponent<FlitController>();
            yield return new WaitForEndOfFrame();
            newFlit.setState(FlitState.splitting);
            newFlit.startSplitMovement(otherNewPos);
            //print(String.Format("new {0} created at {1}, sent to {2}", newFlit.name, newFlit.transform.position.ToString(), otherNewPos.ToString()));

                
            StartCoroutine("splitMov", myNewPos); //state and counter reset
        }else{
            state = FlitState.idle; 
        }
        yield return null;

    }
    public void purge()
    {
        state = FlitState.dying;
        StopAllCoroutines();
        deathAnimation.playDeathAnimation(deathDelegate);
        manager.giveFlitXP();
    }
    private void deathCallback(){

        manager.flitKilled(fController);
        if(!(recycler == null))
            recycler.dumpFlit(this);
        gameObject.SetActive(false);

    }
 
    private IEnumerator moveTo(Vector3 args) //args is a Vector2 destination packed with a float speed
    {
        //print("coroutine started");
        Vector2 dest = (Vector2)args;
        float speed = args.z;
        //we can assume no other coroutine is running
        //wait until previous tween done, this may change
        while (ltidMov != 0 && LeanTween.isTweening(ltidMov)) yield return null;
        //while we're not within the distance error, keep doing the small tweens 
        while(Vector2.Distance(transform.position, dest) > distError)
        {
            //while the tween is not defined or we're not tweening
            //boolean logic is weird, if either of these statements is false,
            //this whole if condition will run. Nand is equal to notted ors.
            if (!(ltidMov != 0 && LeanTween.isTweening(ltidMov)))
            {
                
                Vector2 movTarg = Vector2.MoveTowards(transform.position, dest, flitDistance);
                // print("movTarg: " + movTarg.ToString() + " currpos: " + transform.position);
                ltidMov = LeanTween.move(gameObject, movTarg,
                         flitDistance/speed)
                           .setEaseInQuad().setEaseOutSine().id;
                scaler.move(new Vector3(movTarg.x, movTarg.y, flitDistance/speed));

            }
            yield return null;
        }
        //print("moveTo finished");
        state = FlitState.idle;
        
        //need to set next tween as a ratio between currpos and destpos
    }
    private IEnumerator splitMov(Vector2 args) //args is a Vector2 destination 
    {
        
        /*
        if(!startHasRun){
            Start();
        }
        else{
            manager.flitCreated(fController);
        }*/
        float speed = maxSpeed * splitSpeedRatio;
        //we can assume no other coroutine is running
        //wait until previous tween done, this may change
        while (ltidMov != 0 && LeanTween.isTweening(ltidMov)) { yield return null;  }

        
        ltidMov = LeanTween.move(gameObject, args,
                    splitDistance / speed)
                    .setEaseInQuad().setEaseOutSine().id;
        scaler.move(new Vector3(args.x,args.y,splitDistance/speed));

        yield return new WaitForSeconds(splitDistance / speed);
        
        state = FlitState.idle;
        splitTimer = getSplitTime(minSplitTimer,maxSplitTimer);
        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Similar to FlitMovement, should be combined.
// Slightly different logic in a couple places (walls don't split) 
// so didn't generalize for the original prototype
public class WallMovement : MonoBehaviour
{
    [SerializeField]
    public float maxMoveDist = 1f;
    [SerializeField]
    private int ltidMov = 0;
    [SerializeField]
    private float distError = 0.02f;
    [SerializeField]
    public float flitDistance = 1f;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private int randMovOdds = 100;
    [SerializeField]
    private float randMovDistMax;
    [SerializeField]
    private float randMovSpeedRatio;
    [SerializeField]
    private float resetMovSpeedRatio;
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
    private float wallAvoidanceWeight = 1;
    private Vector2 startPos;
    private Collider2D[] pSpaceHits = new Collider2D[20];

    private MotionScaling scaler;
    public enum WallState
    {
        resetting, spacing, randmoving, idle
    }
    public WallState state = WallState.idle;
    public Collider2D coll;

    void Start()
    {
        scaler = GetComponent<MotionScaling>();
        coll = GetComponent<Collider2D>();
        startPos = transform.position;
        state = WallState.idle;
        
    }

    // Update is called once per frame
    void Update()
    {
        checkStateOverrides();
    }
    private void checkStateOverrides()
    {
        //coroutines will return state to idle when finished
        switch (state)
        {
            case WallState.spacing:
                //when properly spaced, will be set back to idle
                break;
            case WallState.randmoving:
                //sets state to spacing if not properly spaced   
                checkForReset();
                checkPSpace();
                break;
            case WallState.idle:
                checkPSpace();
                checkForReset();
                randOdds(); //sets state to randMoving if odds good
                
                    
                break;
            default:
                break;
        }
    }
    private void checkForReset(){
        //check total distance from startPos we've moved,
        //move back (reset) if it's too much
        float dist = Vector2.Distance(startPos, transform.position);
        if(dist > maxMoveDist){
            StopCoroutine("moveTo");
            StartCoroutine("moveTo", new Vector3(startPos.x, startPos.y, maxSpeed * resetMovSpeedRatio));
            state = WallState.resetting; 
        }

    }

    private void randOdds()
    {
        int randInt = UnityEngine.Random.Range(0, (int) randMovOdds);
        if (randInt == 0)
        {
            StopCoroutine("moveTo");
            Vector2 randPoint = (UnityEngine.Random.insideUnitCircle * randMovDistMax) + new Vector2(transform.position.x, transform.position.y);

            StartCoroutine("moveTo", new Vector3(randPoint.x, randPoint.y, maxSpeed * randMovSpeedRatio));
            state = WallState.randmoving;
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
            if (state == WallState.idle  
                 || hitObjs.Contains("Wall"))
            {
                moveVector = getMoveVector(pSpaceHits, res);
                state = WallState.spacing;
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

    private Vector2 getMoveVector(Collider2D[] hits, int hitNum)
    {
        Vector2 res = new Vector2();
        
        for(int i = 0; i < hitNum; i ++)
        {
            if(hits[i].tag == "Wall"){

                res += getAvoidanceVector(hits[i].transform.position, hitNum, wallAvoidanceWeight);
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
    private IEnumerator moveTo(Vector3 args) //args is a Vector2 destination packed with a float speed
    {
        Vector2 dest = (Vector2)args;
        float speed = args.z;
        //we can assume no other coroutine is running
        //wait until previous tween done, this may change
        while (ltidMov != 0 && LeanTween.isTweening(ltidMov)) yield return null;
        //while we're not within the distance error, keep doing the small tweens 
        while(Vector2.Distance(transform.position, dest) > distError)
        {
            //while the tween is not defined or we're not tweening
            //if either of these statements is false,
            //this whole if condition will run. Because Nand is equal to notted ors.
            if (!(ltidMov != 0 && LeanTween.isTweening(ltidMov)))
            {
                
                Vector2 movTarg = Vector2.MoveTowards(transform.position, dest, flitDistance);
                ltidMov = LeanTween.move(gameObject, movTarg,
                         flitDistance/speed)
                           .setEaseInQuad().setEaseOutSine().id;
                scaler.move(new Vector3(movTarg.x, movTarg.y, flitDistance/speed));

            }
            yield return null;
        }
        state = WallState.idle;
        
    }
}

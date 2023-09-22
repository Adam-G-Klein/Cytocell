using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public List<TrailController> collidedTrails = new List<TrailController>();
    public TrailCollapser collapser;
    private Collider2D coll;
    public bool trailPlaced;
    public bool dead;
    public int trailNum;
    public bool trailCrossed;
    public ObjectRecycler recycler;

    public bool debugPrint = false;
    [SerializeField]
    public Color normalColor = Color.white;
    [SerializeField]
    private Color flitKillColor = Color.blue;
    [SerializeField]
    private float toFlitKillColorTime = 0.2f;
    [SerializeField]
    private float fromFlitKillColorTime = 1.5f;
    private int killFlitLtid = -1;
    private int deathFadeLtid = -1;
    private int nextToDieLtid = -1;
    [SerializeField]
    private float fadeAlpha = 0.2f; 
    [SerializeField]
    private float fadeTime = 0.5f;
    private SpriteRenderer rend;
    public Vector2[] endpoints
    {
        get {
            // quadrants: 1 for top right, 4 bottom right, they run counter clockwise
            int quadrant;
            float rot = transform.eulerAngles.z;

            if (rot >= 180)
                quadrant = rot >= 270f ? 4 : 3;
            else
                quadrant = rot >= 90f ? 2 : 1;

            Collider2D coll = GetComponent<Collider2D>();
            Vector2 center = coll.bounds.center;
            Vector2 ext = coll.bounds.extents;

            Vector2 wp1;
            Vector2 wp2;
            //do some experiment based manipulation, if it's rotated these ways the method above finds the wrong points
            switch (quadrant)
            {
                case 1:
                    wp1 = center - ext;
                    wp2 = center + ext;
                    break;
                case 3:
                    wp1 = center + ext;
                    wp2 = center - ext;
                    break;

                case 2:
                    wp1 = new Vector2(center.x + ext.x, center.y - ext.y);

                    wp2 = new Vector2(center.x - ext.x, center.y + ext.y);
                    break;

                case 4:
                    wp1 = new Vector2(center.x - ext.x, center.y + ext.y);
                    wp2 = new Vector2(center.x + ext.x, center.y - ext.y);
                    break;

                default:
                    wp1 = Vector2.zero;
                    wp2 = Vector2.zero;
                    break;

            }

            return new Vector2[] { wp1, wp2 };
        }
    }

    // Use this for initialization
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        #region Debug Print for end points
        /*if (trailPlaced && !debugPrint)
        {
            Collider2D coll = GetComponent<Collider2D>();
            Vector2 center = coll.bounds.center;
            float halfLen = coll.bounds.extents.x;
            Vector3 local1 = new Vector3(halfLen, 0,0);
            Vector3 local2 = new Vector3(-halfLen,0,0);
            Vector2 worldpt1 = center + (Vector2) coll.bounds.extents;
            Vector2 worldpt2 = center - (Vector2) coll.bounds.extents;

            //print(string.Format("trailNum {0} center {1} bounds extents {2} and bound points {3} and {4}",
                //trailNum, center, coll.bounds.extents, worldpt1,worldpt2));
            Vector2[] ends = endpoints;
            //print(string.Format("trailNum {0} bounds extents {1} and bound points {2} and {3}", 
               //trailNum,coll.bounds.extents, ends[0], ends[1]));
            debugPrint = true;
        }*/
        #endregion
    
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Trails"))
        {
            TrailController otherCont = other.GetComponent<TrailController>();
            //don't want to collapse if neighboring or other one is fading
            if (this.trailNum < otherCont.trailNum
                && Mathf.Abs(otherCont.trailNum - trailNum) != 1
                && !otherCont.dead
                && !this.dead)
            {
                trailCrossed = true;
                //stop this from being called twice
                otherCont.dead = true;   
                collapser.triggerCollapse();

            }
            
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("3nemies"))
        {
            if(trailPlaced && !this.dead) {
                other.GetComponent<FlitController>().purgeNoScoreOrXP();
                StartCoroutine("killFlitAnimation");
            }
        }
    }

    public bool isTrailPlaced(){
        return trailPlaced;
    }
    private void OnEnable(){
        dead = false;
        trailPlaced = false;
        trailCrossed = false;

        StopCoroutine("fade");
        StopCoroutine("deathFade");
        SpriteRenderer image = GetComponent<SpriteRenderer>();
        Color tempColor = image.color;
        tempColor.a = 1f;
        image.color = tempColor;
}
    private void OnDisable()
    {
        dead = true;
        StopAllCoroutines();
        if(deathFadeLtid != -1 && LeanTween.isTweening(deathFadeLtid)) LeanTween.cancel(deathFadeLtid);
        if(nextToDieLtid != -1 && LeanTween.isTweening(nextToDieLtid)) LeanTween.cancel(nextToDieLtid);
        if(killFlitLtid != -1 && LeanTween.isTweening(killFlitLtid)) LeanTween.cancel(killFlitLtid);
        recycler.trails.Enqueue(gameObject);
        trailCrossed = false;
        trailNum = -1;
        trailPlaced = false;
    }
    public void nextToDie(){

        StartCoroutine("nextToDieFade",0.2);
    }

    public void die(){
        //used for trails outside maxTrails
        //slowly die and then disable
        //right now will just fade and then disable
        dead = true;
        StartCoroutine("deathFade");
        
    }

    IEnumerator deathFade(){
        if(killFlitLtid != -1 && LeanTween.isTweening(killFlitLtid))
            LeanTween.cancel(killFlitLtid);
        if(nextToDieLtid != -1 && LeanTween.isTweening(nextToDieLtid))
            LeanTween.cancel(nextToDieLtid);
        deathFadeLtid = LeanTween.alpha(gameObject,0,Mathf.Lerp(0, fadeTime, rend.color.a)).id;
        // can't wait while tweening because we use the same ltid
        // for nexttodie and deathfade
        yield return new WaitForSeconds(fadeTime);
        gameObject.SetActive(false);
    }

    IEnumerator nextToDieFade(float final){
        if(killFlitLtid != -1 && LeanTween.isTweening(killFlitLtid))
            LeanTween.cancel(killFlitLtid);
        nextToDieLtid = LeanTween.alpha(gameObject,final,
            fadeTime).id;
        yield return null;
    }

    IEnumerator killFlitAnimation(){
        if(deathFadeLtid != -1 && LeanTween.isTweening(deathFadeLtid))
            yield break;
        if(nextToDieLtid != -1 && LeanTween.isTweening(nextToDieLtid)) {
            Color toColor = Color.Lerp(new Color(normalColor.r, normalColor.g, normalColor.b, fadeAlpha),flitKillColor,0.5f);
        } else {
            Color toColor = flitKillColor;
        }
        killFlitLtid = LeanTween.color(gameObject,flitKillColor,toFlitKillColorTime).id;
        yield return new WaitWhile(()=>LeanTween.isTweening(killFlitLtid));
        killFlitLtid = LeanTween.color(gameObject,normalColor,fromFlitKillColorTime).id;
        yield return new WaitWhile(()=>LeanTween.isTweening(killFlitLtid));
    }
    public void setFadeTime(float newFadeTime){
        fadeTime = newFadeTime;
    }




}

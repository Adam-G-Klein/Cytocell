using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyMotionScaler : MonoBehaviour
{
    public Vector3 unchangedScale = new Vector3(1,1,1);
    /* [SerializeField]
    private float minScale;*/
    [SerializeField]
    private float maxScale;
    [SerializeField]
    private float prepRatio;
    [SerializeField]
    private float movRatio;
    [SerializeField]
    private float normalizeRatio;

    private int ltid = 0;
    //added to y, subtracted from x;
    private float scaleFactor;
    private PlayerSwiper pswipe;
    private float flitDist;
    private bool scalerCoroutRunning = false;
    public float scaleFactorTwitchRatio = 0.5f;
    private Transform sprite;
    /*assumes that preparingToMove() and move() are called at appropriate times */
    void Start()
    {
        sprite = transform.GetComponentInChildren<SpriteRenderer>().transform;
        pswipe = GetComponent<PlayerSwiper>(); 
        flitDist = pswipe.swipeLength;
    }



    //args: (dest.x, dest.y, movTime)
    public void move(float movTime){
        StopAllCoroutines();
        if(ltid != 0 && LeanTween.isTweening(ltid)){
            LeanTween.cancel(ltid);
        }

        //if(!largeScaleDiff())
            StartCoroutine("moveCorout",movTime);
        
    }
    //determines if the corout will cause a weird twitch
    //this happens when swipes occur too quickly
    private bool largeScaleDiff(){
        return scaleFactor > scaleFactorTwitchRatio * maxScale;
    }
    IEnumerator moveCorout(float movTime){
        scalerCoroutRunning = true;
        float scaleFactor =  (sprite.localScale.y - sprite.localScale.x)/2;
        
        float realScaleFactorReached = (movTime / flitDist) * maxScale;
        //note the scalefactor goes negative here, jelly is oriented differently
        ltid = LeanTween.value(
                gameObject, scaleFactor, -1 * realScaleFactorReached, movTime * prepRatio)
                .setOnUpdate((float val) => {
                    scaleFactor = val;
                    sprite.localScale = new  Vector3(unchangedScale.x - scaleFactor,
                                            unchangedScale.y + scaleFactor,0);
                    }).id;
        while(LeanTween.isTweening(ltid)) yield return null;
        ltid = LeanTween.value(
                gameObject, scaleFactor, realScaleFactorReached, movTime * movRatio)
                .setOnUpdate((float val) => {
                    scaleFactor = val;
                    sprite.localScale = new  Vector3(unchangedScale.x - scaleFactor,
                                            unchangedScale.y + scaleFactor,0);
                    }).id;
        while(LeanTween.isTweening(ltid)){
            yield return null;
        }
        ltid = LeanTween.value(
                gameObject,  
                scaleFactor, 0, movTime * normalizeRatio)
                .setOnUpdate((float val) => {
                    scaleFactor = val;
                    sprite.localScale = new  Vector3(unchangedScale.x - scaleFactor,
                                            unchangedScale.y + scaleFactor,0);

                    })
                .id;
        while(LeanTween.isTweening(ltid)){
            yield return null;
        }
        scalerCoroutRunning = false;
        yield return null;

    }
}


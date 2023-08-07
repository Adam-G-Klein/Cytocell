using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Responsible for the procedural animation of the player 
public class JellyMotionScaler : MonoBehaviour
{
    public Vector3 unchangedScale = new Vector3(1,1,1);
    [SerializeField]
    private float maxScale;
    [SerializeField]
    private float prepTimeRatio;
    [SerializeField]
    private float movTimeRatio;
    [SerializeField]
    private float normalizeTimeRatio;
    /// <summary>
    /// Multiplies (movDist/movTime) to get the number we add and subtract from the scale
    /// during tweening
    /// </summary>
    [SerializeField]
    private float targetScaleMultiplier = 0.2f;

    private int ltid = 0;
    //added to y, subtracted from x;
    private float scaleFactor;
    private PlayerSwiper pswipe;
    private float flitDist;
    private bool scalerCoroutRunning = false;
    private Transform sprite;
    /*assumes that preparingToMove() and move() are called at appropriate times */
    void Start()
    {
        sprite = transform.GetComponentInChildren<SpriteRenderer>().transform;
        pswipe = GetComponent<PlayerSwiper>(); 
    }

    //args: (dest.x, dest.y, movTime)
    public void move(float movTime, float movDist){
        StopAllCoroutines();
        if(ltid != 0 && LeanTween.isTweening(ltid)){
            LeanTween.cancel(ltid);
        }

        StartCoroutine("moveCorout",new Vector2(movTime, movDist));
        
    }

    IEnumerator moveCorout(Vector2 movTimeAndDist){
        float movTime = movTimeAndDist.x;
        float movDist = movTimeAndDist.y;
        scalerCoroutRunning = true;
        float currentScaleFactor =  (sprite.localScale.y - sprite.localScale.x)/2;
        
        float targetScaleFactorPlusAndMinus = Mathf.Clamp((movDist / movTime) * targetScaleMultiplier, 0, maxScale);
        print(string.Format("movDist: {0} movTime: {1} targetScaleMultiplier: {2} maxScale: {3} targetScaleFactorPlusAndMinus: {4}", movDist, movTime, targetScaleMultiplier, maxScale, targetScaleFactorPlusAndMinus));
        //note the scalefactor goes negative here, jelly is oriented differently
        //TODO: break these tweens into separate methods. Access information from a shared
        // object reference

        // tween the scalefactor to squish the player horizontally before the movement 
        ltid = LeanTween.value(
                gameObject, currentScaleFactor, -1 * targetScaleFactorPlusAndMinus, movTime * prepTimeRatio)
                .setOnUpdate((float val) => {
                    currentScaleFactor = val;
                    sprite.localScale = new  Vector3(unchangedScale.x - currentScaleFactor,
                                            unchangedScale.y + currentScaleFactor,0);
                    }).id;
        while(LeanTween.isTweening(ltid)) yield return null;
        // tween the scalefactor to stretch the player vertically during the movement
        ltid = LeanTween.value(
                gameObject, currentScaleFactor, targetScaleFactorPlusAndMinus, movTime * movTimeRatio)
                .setOnUpdate((float val) => {
                    currentScaleFactor = val;
                    sprite.localScale = new  Vector3(unchangedScale.x - currentScaleFactor,
                                            unchangedScale.y + currentScaleFactor,0);
                    }).id;
        while(LeanTween.isTweening(ltid)){
            yield return null;
        }
        // get the player back to normal
        ltid = LeanTween.value(
                gameObject,  
                currentScaleFactor, 0, movTime * normalizeTimeRatio)
                .setOnUpdate((float val) => {
                    currentScaleFactor = val;
                    sprite.localScale = new  Vector3(unchangedScale.x - currentScaleFactor,
                                            unchangedScale.y + currentScaleFactor,0);

                    })
                .id;
        while(LeanTween.isTweening(ltid)){
            yield return null;
        }
        scalerCoroutRunning = false;
        yield return null;

    }
}


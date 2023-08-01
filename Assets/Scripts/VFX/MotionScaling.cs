using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// scales the enemies based on their motion, one component
// of their procedural animation
public class MotionScaling : MonoBehaviour
{
    [SerializeField]
    private Vector3 unchangedScale = new Vector3(1,1,1);
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
    private FlitController controllerf;
    private WallMovement controllerw;
    private float flitDist;
    /*assumes that preparingToMove() and move() are called at appropriate times */
    void Start()
    {
        controllerf = GetComponent<FlitController>(); 
        if(controllerf == null){ 
            controllerw = GetComponent<WallMovement>(); 
            flitDist = controllerw.flitDistance;
        }
        else{  
            flitDist = controllerf.flitDistance;
        }
    }



    //args: (dest.x, dest.y, movTime)
    public void move(Vector3 args){
        StopAllCoroutines();

        StartCoroutine("moveCorout",args);
        
    }
    IEnumerator moveCorout(Vector3 args){
        float scaleFactor =  (transform.localScale.y - transform.localScale.x)/2;
        if(ltid != 0 && LeanTween.isTweening(ltid)){
            LeanTween.cancel(ltid);
        }
        float newAngle = Mathf.Atan2(transform.position.y - args.y,
                                     transform.position.x - args.x)
                                     * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);

        float movDist = Vector2.Distance((Vector2) transform.position,
                                        (Vector2) args);
        if(flitDist == 0){
            Start();
        }
        float realScale = (movDist / flitDist) * maxScale;
        ltid = LeanTween.value(
                gameObject, scaleFactor, realScale, args.z * prepRatio)
                .setOnUpdate((float val) => {
                    scaleFactor = val;
                    transform.localScale = new  Vector3(unchangedScale.x - scaleFactor,
                                            unchangedScale.y + scaleFactor,0);
                    }).id;
        while(LeanTween.isTweening(ltid)) yield return null;
        ltid = LeanTween.value(
                gameObject, scaleFactor, -1 * realScale, args.z * movRatio)
                .setOnUpdate((float val) => {
                    scaleFactor = val;
                    transform.localScale = new  Vector3(unchangedScale.x - scaleFactor,
                                            unchangedScale.y + scaleFactor,0);
                    }).id;
        while(LeanTween.isTweening(ltid)){
            yield return null;
        }
        ltid = LeanTween.value(
                gameObject,  
                scaleFactor, 0, args.z * normalizeRatio)
                .setOnUpdate((float val) => {
                    scaleFactor = val;
                    transform.localScale = new  Vector3(unchangedScale.x - scaleFactor,
                                            unchangedScale.y + scaleFactor,0);

                    })
                .id;
        yield return null;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeIndicatorController : MonoBehaviour
{
    Material arrowMat;
    [SerializeField]
    Vector2 rotationCenterOffset;
    Vector2 rotationCenter;
    [SerializeField]
    float distanceFromCenter;
    [SerializeField]
    float minSideCircleRadius = 1.5f;
    [SerializeField]
    float maxSideCircleRadius = 1.65f;
    public Vector2 currPointDir;
    private SwipeController swipeController;
    private float currMagnitude;
    private Transform playerTrans;
    [SerializeField]
    private float maxMagnitude;
    [SerializeField]
    private float minAnimationSpeed;
    [SerializeField]
    private float maxAnimationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTrans = playerGO.transform;
        swipeController = playerGO.GetComponent<SwipeController>();
        arrowMat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        currMagnitude = swipeController.getDir().magnitude;
        if(currMagnitude > 0) {
            arrowMat.SetInt("_visible", 1);
            rotationCenter = (Vector2)playerTrans.position + rotationCenterOffset;
            currPointDir = swipeController.getDir().normalized;
            rotateToPointDir();
            positionOnPointDir();
            float magnitudeRatio = currMagnitude  / maxMagnitude; 
            resizeOnMagnitude(magnitudeRatio);
        } else {
            arrowMat.SetInt("_visible", 0);
        }
        
    }

    void rotateToPointDir(){
        float angle = Quaternion.FromToRotation(Vector3.up, currPointDir).eulerAngles.z;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void positionOnPointDir(){
        Vector2 pos = rotationCenter + (currPointDir * distanceFromCenter);
        transform.position = pos;
    }

    void resizeOnMagnitude(float magnitudeRatio){
        // go from max to min because the arrow gets bigger if the radius is smaller
        float radius = Mathf.Lerp(maxSideCircleRadius, minSideCircleRadius, magnitudeRatio);
        arrowMat.SetFloat("_sideCirclesRadius", radius);
    }

}

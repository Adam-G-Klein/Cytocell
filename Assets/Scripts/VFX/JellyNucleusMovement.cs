using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//moves a cellalpha shader's nucleus based on its scaling
// See MotionScaling.cs
//assumes the material is a component of the child
public class JellyNucleusMovement : MonoBehaviour
{
    public float nucleusMovRatio = 1;
    private JellyShaderController shade;
    private JellyMotionScaler motionScaler;
    public float xFlangeRatio = 1;
    public float yFlangeRatio = 1;
    private float epDistRatio = 1;

    private float initXFlange = 1;
    private float initYFlange = 1;
    private float initEpDist = 1;
    private float xScaleDiff = 0;
    private float yScaleDiff = 0;

    private Transform spriteTrans;
    
    // Start is called before the first frame update
    void Start()
    {
        shade = GetComponent<JellyShaderController>();
        motionScaler = GetComponentInParent<JellyMotionScaler>();
        spriteTrans = GetComponentInChildren<SpriteRenderer>().transform;
        initYFlange = shade.yFlange;
        initXFlange = shade.xFlange;
        initEpDist = shade._nucleiEpicenterDistance;
        epDistRatio = PurchaseManager.instance.getEquippedSkin().nucleusMovement;
    }

    // Update is called once per frame
    void Update()
    {
        xScaleDiff = motionScaler.unchangedScale.x - spriteTrans.localScale.x;
        yScaleDiff = motionScaler.unchangedScale.y - spriteTrans.localScale.y;
        shade.xFlange = initXFlange + (xScaleDiff * xFlangeRatio);
        shade.yFlange = initYFlange + (yScaleDiff * yFlangeRatio);
        shade._nucleiEpicenterDistance = initEpDist + (yScaleDiff * epDistRatio);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyNucleusMovement : MonoBehaviour
{
    //moves a cellalpha shader's nuucleus based on its scaling
    //assumes the material is a component of the child
    public float nucleusMovRatio = 1;
    private JellyShaderController shade;
    private JellyMotionScaler motionScaler;
    public float xFlangeRatio = 1;
    public float yFlangeRatio = 1;
    public float epDistRatio = 1;

    private float initXFlange = 1;
    private float initYFlange = 1;
    private float initEpDist = 1;
    private float xScaleDiff = 0;
    private float yScaleDiff = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        shade = GetComponentInChildren<JellyShaderController>();
        motionScaler = GetComponentInChildren<JellyMotionScaler>();
        initYFlange = shade.yFlange;
        initXFlange = shade.xFlange;
        initEpDist = shade._nucleiEpicenterDistance;
    }

    // Update is called once per frame
    void Update()
    {
        xScaleDiff = motionScaler.unchangedScale.x - transform.localScale.x;
        yScaleDiff = motionScaler.unchangedScale.y - transform.localScale.y;
        shade.xFlange = initXFlange + (xScaleDiff * xFlangeRatio);
        shade.yFlange = initYFlange + (yScaleDiff * yFlangeRatio);
        shade._nucleiEpicenterDistance = initEpDist + (yScaleDiff * epDistRatio);
           

        

        
    }
}

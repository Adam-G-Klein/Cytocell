using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleusMovement : MonoBehaviour
{
    public float nucleusMovRatio = 1;
    public Vector4 initialNucleusPos = new Vector4(0.5f,0.5f,0.5f,0);
    public Vector4 initialScale = new Vector4(1f,1f,1f,0);
    [SerializeField]
    private Material _mat;


    // Start is called before the first frame update
    void Start()
    {
        _mat = GetComponentInChildren<SpriteRenderer>().material;
        if(_mat == null)
            //for the case that this is on a wall, where the spriterenderer will be a component of this object
            _mat = GetComponent<SpriteRenderer>().material;
        
    }

    // Update is called once per frame
    void Update()
    {

        float forwardDiff = Mathf.Clamp(((initialScale.y - transform.localScale.y) * nucleusMovRatio) + 
                initialNucleusPos.x,0,1);
        Vector4 nucleusVector = new Vector4(forwardDiff, initialNucleusPos.y, initialNucleusPos.z, 0);
        _mat.SetVector("_nucleusLocation", nucleusVector);
        

        
    }
}

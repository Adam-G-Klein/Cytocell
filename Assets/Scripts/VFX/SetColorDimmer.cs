using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorDimmer : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer rend;
    [SerializeField]
    float val;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.material.SetFloat("_colorDimmer", val);
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundValueSetter : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer rend;
    [SerializeField]
    float dimmness ;
    [SerializeField]
    float size= -11.68f;
    [SerializeField]
    Color backgroundColor;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.material.SetFloat("_colorDimmer", dimmness);
        rend.material.SetFloat("_Size", size);
        rend.material.SetColor("_BackColor", backgroundColor);
    }

    void Update() {
        rend.material.SetFloat("_colorDimmer", dimmness);
        rend.material.SetFloat("_Size", size);
        rend.material.SetColor("_BackColor", backgroundColor);

    }
}

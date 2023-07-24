using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailConstructedMsg : MonoBehaviour
{
    private XPBarController xpBar;
    private Vector3 startworldpos;
    public float endPos;
    public float endAlpha;
    private 
    // Start is called before the first frame update
    void Start()
    {
        xpBar = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<XPBarController>();
        startworldpos = transform.position;

        
    }

    

   

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailNodeController : MonoBehaviour
{
    private int ltid = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void move(Vector2 newLoc, float movTime){

        if(ltid != 0 && LeanTween.isTweening(ltid)){
            LeanTween.cancel(ltid);
        }
        ltid = LeanTween.move(gameObject,newLoc,movTime).id;

        //StopCoroutine("moveCorout");
        //StartCoroutine("moveCorout", new Vector3(newLoc.x, newLoc.y, speed));
    }

/* 
    private IEnumerator moveCorout(Vector3 packedArgs){
        Vector2 dest = new Vector2(packedArgs.x, packedArgs.y);
        float speed = packedArgs.z;

        while()

    }
    */
}

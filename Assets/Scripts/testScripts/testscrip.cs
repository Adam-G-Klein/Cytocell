using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class testscrip : MonoBehaviour {

    public float radius = 10;
    public float distanceToObstacle = 0;
    Collider2D coll;

	// Use this for initialization
	void Start () {
        coll = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Collider2D[] hits = new Collider2D[20];

        Vector2 p1 = transform.position;


        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        //print("test running");
        coll.enabled = false;
        int res = Physics2D.OverlapCircleNonAlloc(p1, radius,hits);
        coll.enabled = true;
       
        if (res > 0)
        {
            //distanceToObstacle = hit.distance;
            
            for(int i = 0; i < res; i++)
            {

                print("hit " + i
                    + " object: " + hits[i].gameObject.tag);

            }
            Array.Clear(hits, 0, hits.Length);
        }
    }
}

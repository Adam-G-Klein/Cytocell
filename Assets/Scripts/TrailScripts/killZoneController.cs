using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killZoneController : MonoBehaviour {

    private Collider2D coll;

	// Use this for initialization
	void Start () {
        coll = GetComponent<EdgeCollider2D>();




	}
	
	// Update is called once per frame
	void Update () {
		
	}


}

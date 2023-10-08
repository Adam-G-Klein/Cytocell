using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform player;
    private Vector2 playerPos;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        playerPos = player.position;
        transform.position = new Vector3
            (playerPos.x, playerPos.y, -10f);
	}
}

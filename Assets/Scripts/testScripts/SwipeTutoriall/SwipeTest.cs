using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTest : MonoBehaviour {

    public Swipe swipeControls;
    public Transform player;
    private Vector3 desiredPos;


	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {

        if (swipeControls.SwipeLeft)
            desiredPos += Vector3.left;
        if (swipeControls.SwipeRight)
            desiredPos += Vector3.right;
        if (swipeControls.SwipeUp)
            desiredPos += Vector3.up;
        if (swipeControls.SwipeDown)
            desiredPos += Vector3.down;

        player.transform.position = Vector3.MoveTowards(player.transform.position, desiredPos, 3f * Time.deltaTime);
    }
}

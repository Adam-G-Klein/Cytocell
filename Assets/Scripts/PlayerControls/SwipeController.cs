using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles taking and translating input into the swipes
// that every other system cares about
public class SwipeController : MonoBehaviour
{

    public bool isDragging;
    private Vector2 startTouch, swipeDelta, endTouch;
    public float deadZoneRadius = 125f;
    private float test;

    public Vector2 startPos;
    public Vector2 lastDir;
    private Vector2 currDir, touchPoint, dirRunningAvg;
    public bool swiped;

    void Update()
    {
        // single frame leading-edge state change
        // TODO: event bus system so other systems don't need to poll for this
        if (swiped)
        {
            isDragging = swiped = false;
            currDir = Vector2.zero;
            
        }
        #region Standalone Inputs
        if (Input.GetMouseButton(0))
        {
            if(!isDragging) startPos = (Vector2)Input.mousePosition;
            touchPoint = (Vector2)Input.mousePosition;
            dirRunningAvg = touchPoint - startPos;
            isDragging = true;

        }
        else if (isDragging)
        {
            isDragging = false;
            currDir = (Vector2)Input.mousePosition - startPos;
            

        }
        else
        {
            startPos = currDir = Vector2.zero;
            
        }
        #endregion

        #region Mobile Inputs
        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Handle finger movements based on touch phase.
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    startPos = touch.position;
                    swiped = false;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    currDir = touch.position - startPos;
                    touchPoint = touch.position;
                    dirRunningAvg = touchPoint - startPos;
                    break;

                case TouchPhase.Ended:
                    if(dirRunningAvg.magnitude > deadZoneRadius) {
                        swiped = true;
                        lastDir = currDir;
                    }
                    break;

            }
        }
        else {
            startPos = currDir = Vector2.zero;
        }

        #endregion

        //Calculate the distance
        //did we cross deadzone?
        
        

    }

    public Vector2 getDir()
    {
        return currDir;
    }


}




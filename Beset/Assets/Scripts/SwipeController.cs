using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{

    public bool isDragging;
    private Vector2 startTouch, swipeDelta, endTouch;
    public float deadZoneRadius = 125f;

    public Vector2 startPos;
    public Vector2 lastDir;
    private Vector2 currDir, touchPoint, dirRunningAvg;
    public bool swiped;

    void Update()
    {
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
            swiped = true;
            lastDir = currDir;
            Debug.Log(string.Format("mousePos: {0} startPos: {1} mpos-spos: {2} dir: {3} dirMag: {4}", 
            (Vector2) Input.mousePosition,startPos, (Vector2) Input.mousePosition - startPos, 
               direction.ToString(), direction.magnitude));

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
            print("went into mobile controls");
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
                // next step for swipe overhaul: indicator reads from currDir
                case TouchPhase.Ended:
                    swiped = true;
                    lastDir = currDir;
                    break;

            }
        }

        #endregion

        //Calculate the distance
        
        
        //did we cross deadzone?
       
        
        /*
        if (currDir.magnitude > deadZoneRadius)
        {
            //which dir?
            swiped = true;
            lastDir = currDir;
            //Debug.Log(string.Format("swiped, startPos: {0}, mousPos: {1}, dir: {2}", startPos, (Vector2)Input.mousePosition, direction.ToString()));
        }
        */
        

    }


}




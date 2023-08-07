using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsidePolygonUtil : MonoBehaviour {



    public static bool InsidePolygon(Vector2[] polygon, Vector2 p)
    {
        int i;
        int n = polygon.Length;
        float angle = 0;
        Vector2 p1, p2;


        for (i = 0; i < n; i++)
        {
            //the xval for p1 is the xval of the i'th point
            //of the polygon minus the x val of the point
            //we're checking against 
            //p1 will be the vector from the checked point
            //to the i'th polygon point
            p1.x = polygon[i].x - p.x;
            p1.y = polygon[i].y - p.y;
            //p2 is the difference between our checked point
            //and the i+1th polygon point
            //this will be the vector from the checked point to the
            //next polygon point (or the first, if we're on the last one)
            p2.x = polygon[(i + 1) % n].x - p.x;
            p2.y = polygon[(i + 1) % n].y - p.y;
            //add the angle between these two values to the running total
            angle += Angle2D(p1.x, p1.y, p2.x, p2.y);
        }
        

        return !(Mathf.Abs(angle) < Mathf.PI);
    }

    /*
       Return the angle between two vectors on a plane
       The angle is from vector 1 to vector 2, positive anticlockwise
       The result is between -pi -> pi
    */
    static float Angle2D(float x1, float y1, float x2, float y2)
    {
        float dtheta, theta1, theta2;

        theta1 = Mathf.Atan2(y1, x1);
        theta2 = Mathf.Atan2(y2, x2);
        dtheta = theta2 - theta1;
        while (dtheta > Mathf.PI)
            dtheta -= Mathf.PI*2;
        while (dtheta < -Mathf.PI)
            dtheta += Mathf.PI * 2;

        return dtheta;
    }

    
}

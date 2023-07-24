using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class boundsIntersectTest : MonoBehaviour
{
    public GameObject m_MyObject;//, m_NewObject;
    Collider2D m_Collider2D;//, m_Collider2D2;
    ContactFilter2D filter;
    Collider2D[] results = { };
    void Start()
    {
        //Check that the first GameObject exists in the Inspector and fetch the Collider2D
        if (m_MyObject != null)
        {

            m_Collider2D = m_MyObject.GetComponent<Collider2D>();
        }
        //Check that the second GameObject exists in the Inspector and fetch the Collider2D
        /*if (m_NewObject != null)
        {
            m_Collider2D2 = m_NewObject.GetComponent<Collider2D>();
            print("obj 2 inst");
        }*/
        filter = new ContactFilter2D();
        //filter.NoFilter();
        filter.SetLayerMask(LayerMask.GetMask("Enemies"));
        filter.useTriggers = true;
        
    }

    void Update()
    {
        //If the first GameObject's Bounds enters the second GameObject's Bounds, output the message
        

/*
        m_Collider2D.OverlapCollider(filter,results);

        //if (filter.isFiltering) print("im filtering homie");
        if (results.Length > 0)
        {
            print("results: " + results.ToString());
        }
        */
        /*
        if (m_Collider2D.bounds.Intersects(m_Collider2D2.bounds))
        {
            Debug.Log("Bounds intersecting");
        }*/
        //print("i work");
        

    }


}
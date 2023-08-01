using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPSController : MonoBehaviour
{
    private Camera cam;
    public float frickenZ;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        print("cam name: " + cam.gameObject.name);
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position =
            cam.ScreenToWorldPoint(new Vector3(
                cam.pixelWidth/2,
                0, 
                frickenZ));
    }

    
}

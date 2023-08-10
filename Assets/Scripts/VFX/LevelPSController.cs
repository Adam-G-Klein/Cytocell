using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPSController : MonoBehaviour
{
    private Camera cam;
    public float zPos;
    public float yOffset;
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ps = GetComponentInChildren<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void emit(){
        transform.position =
            cam.ScreenToWorldPoint(new Vector3(
                cam.pixelWidth/2,
                yOffset, 
                zPos));
        ps.Play();
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyDeathPSController : MonoBehaviour
{
    public GameObject[] particleSystems;
    [SerializeField]
    private GameObject manager;
    [SerializeField]
    private float longestParticleLifetime;
    public bool done;
    // Start is called before the first frame update

/*
    private IEnumerator disableAndRecyleCorout(){
        yield return new WaitForSeconds(longestParticleLifetime);
        done = true;

    }
    */
    


}

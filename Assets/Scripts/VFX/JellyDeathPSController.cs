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

}

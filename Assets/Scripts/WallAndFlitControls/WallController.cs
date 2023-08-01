using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Just used by other systems to poll whether the monobehavior
// reference they have from a collision check is a wall
// and to have a sensible place for them to get knockback constants from
public class WallController : MonoBehaviour
{
    public float knockBack = 3.0f;
    public float knockMovTime = 0.5f;
    public float knockRotTime = 0.5f;
}

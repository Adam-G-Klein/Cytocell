using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailSetController : MonoBehaviour
{

    [SerializeField]
    public List<TailController> tailLst;

    public void move(Vector2 newPlayerLoc){
        foreach(TailController cont in tailLst){
            cont.move(newPlayerLoc);
        }
    }
}

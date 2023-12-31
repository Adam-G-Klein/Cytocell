﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;

public class JellyShaderController : MonoBehaviour
{
    public bool inUse = true;
    public bool shopDisplay = false;
    private Material _mat;
    private List<Vector4> nucPos = new List<Vector4>();
    private List<float> nucAngles = new List<float>();
    public int _numNuclei = 5;
    public float _maxNucleiAngle = 160f;
    public float _minNucleiAngle = 20f;
    public Vector4 _nucleiEpicenterLocation = new Vector4(0.5f,0.5f,0,0);
    public float _nucleiEpicenterDistance = 0.4f;
    public float xFlange = 1f;
    public float yFlange = 1f;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(LateStart());
        
       
    }


    // Update is called once per frame
    void Update()
    {
        if(_mat != null)
            initNuclei();
        
    }
    //find the nucleiPos's
    //the y value is hypotenuse * sin(angle)
    //the x value is hypotenuse * cos(angle)
    //the x value is negative if <90, pos otherwise
    void initNuclei(){
        float _angleRange = _maxNucleiAngle - _minNucleiAngle;
        float _angleStep;
        nucPos.Clear();
        nucAngles.Clear();
        if(_numNuclei % 2 == 0){
            _angleStep = _angleRange / _numNuclei;
        }else{
            _angleStep = _angleRange / (_numNuclei - 1);
        }
        float tmpAngle;
        float xPos;
        float yPos;
        float xDiff;
        float yDiff;
        for(int i = 0; i < _numNuclei; i++ ){

           tmpAngle = _minNucleiAngle + (i*_angleStep);

           xDiff = (_nucleiEpicenterDistance * Mathf.Cos(Mathf.Deg2Rad * tmpAngle))*xFlange;
           yDiff = (_nucleiEpicenterDistance * Mathf.Sin(Mathf.Deg2Rad * tmpAngle))*yFlange;
           xPos = _nucleiEpicenterLocation.x + xDiff;
           yPos = _nucleiEpicenterLocation.y + yDiff;
           if(xDiff <= 0){
               tmpAngle = Mathf.Atan(yDiff/xDiff);
               nucAngles.Add(tmpAngle + Mathf.PI);
           }else{
               nucAngles.Add(Mathf.Atan(yDiff/xDiff));

           }
           nucPos.Add(new Vector4(xPos,yPos,0,0));

        }
        _mat.SetVectorArray("_nucleiPos",nucPos);
        _mat.SetFloatArray("_nucleiAngles",nucAngles);
        _mat.SetVector("_nucleiEpicenterLocation", _nucleiEpicenterLocation);
    }
    IEnumerator LateStart(){
        yield return new WaitForEndOfFrame();
        if(!inUse){
            yield break;
        }
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if(rend == null){
            _mat = GetComponent<Image>().material;
        } else {
            _mat = rend.material;
        }
        _mat.SetInt("_numNuclei", _numNuclei);
        initNuclei();
    }
}

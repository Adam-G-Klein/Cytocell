﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryButton : MonoBehaviour
{
    public UnityEngine.SceneManagement.Scene gameScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clicked(){
        print("annnnd scene!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{

    public bool paused = false;
    private Image rend;
    public Color playColor;
    public Color pauseColor;
    private VisualManager visManage;



    void Start()
    {
        paused = false;
        rend = GetComponent<Image>();
        
    }
    public void clicked()
    {
        if (paused)
        {

            rend.color = playColor;
            Time.timeScale = 1;
            paused = false;
        }
        else
        {
            rend.color = pauseColor;
            Time.timeScale = 0;
            paused = true;

        }

    }
}

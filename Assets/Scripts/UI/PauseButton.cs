﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//for now this is always clickable, and doesn't belong to a button group
public class PauseButton : MonoBehaviour
{
    public bool paused = false;
    private Image rend;
    public Color playColor;
    public Color pauseColor;
    private VisualManager visManage;
    private ButtonGroupAlphaControls pauseMenuControls;
    [SerializeField]
    private GameManager manager;
    private GameObject pauseButton;
    private PauseButton pauseButtonScript;

    public GameObject pauseMenuContainer;
    private PlayerSwiper playerSwiper;


    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rend = GetComponent<Image>();
        pauseMenuContainer = GameObject.FindGameObjectWithTag("PauseGroup");
        pauseMenuControls = pauseMenuContainer.GetComponent<ButtonGroupAlphaControls>();
        playerSwiper = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSwiper>();

    }
    public void clicked()
    {
        if (manager.gamePaused)
        {
            rend.color = playColor;
            Time.timeScale = 1;
            manager.gamePaused = false;
            pauseMenuContainer.SetActive(false);
            pauseMenuControls.hideAll();
            playerSwiper.swipeEnabled = true;
        }
        else
        {
            rend.color = pauseColor;
            Time.timeScale = 0;
            manager.gamePaused = true;
            pauseMenuContainer.SetActive(true);
            pauseMenuControls.displayAll();
            playerSwiper.swipeEnabled = false;
        }

    }
}

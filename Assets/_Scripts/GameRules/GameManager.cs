using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    private GameState state = GameState.STARTING;
    public GameState State { get { return state; } }
    private int timer;
    private int points;
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Header("scriptable object to define section time")]
    [SerializeField] private GameOptions gameOptions;
    private void Awake()
    {
        //Define singleton
        if( instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    void Start()
    {
        state = GameState.STARTING;
        timer = gameOptions.timeSection;
    }

    private void Update()
    {
        if (state == GameState.STARTING && GameInput.Instance.SpaceIsPressed())
        {
            state = GameState.PLAY;
            StartCoroutine(CountTime());
            UIController.Instance.DeactivateInstructions();

        }
    }


    /// <summary>
    /// Counts the time played in the section.
    /// </summary>
    private IEnumerator CountTime()
    {
        while(timer >= 0 && state == GameState.PLAY)
        {
            yield return new WaitForSecondsRealtime(1f);
            timer -= 1;
            UIController.Instance.UpdateTimerTMP(timer);
        }
        GameOver();

    }

    /// <summary>
    /// End the game and call the function to open the game over panel
    /// </summary>
    public void GameOver()
    {
        state = GameState.GAMEOVER;
        GameInput.Instance.gameObject.SetActive(false);
        UIController.Instance.GameOverUI(timer <= 0, points);
    }


    public void AddPoint()
    {
        points += 1;
    }
}

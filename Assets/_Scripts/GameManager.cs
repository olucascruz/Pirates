using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum StateGame
{
    PLAY, GAMEOVER
}

public class GameManager : MonoBehaviour
{
    private StateGame state = StateGame.PLAY;
    public StateGame State { get { return state; } }


    private int timer;
    private int points;
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] private GameOptions gameOptions;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        timer = gameOptions.timeSection;
        StartCoroutine(CountTime());
    }

    private IEnumerator CountTime()
    {
        while(timer >= 0 && state == StateGame.PLAY)
        {
            yield return new WaitForSecondsRealtime(1f);
            timer -= 1;
            UIController.Instance.UpdateTimerTMP(timer);
        }
        GameOver();

    }

    public void GameOver()
    {
        state = StateGame.GAMEOVER;
        GameInput.Instance.gameObject.SetActive(false);
        UIController.Instance.GameOverUI(timer <= 0, points);
    }

    public void AddPoint()
    {
        points += 1;
    }
}

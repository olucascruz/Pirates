using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerTMP;
    [SerializeField] private GameObject canvasGameOver;
    [SerializeField] private TextMeshProUGUI feedbackTMP;
    [SerializeField] private TextMeshProUGUI pointsTMP;
    [SerializeField] private GameObject canvasInstructions;


    [TextArea]
    [SerializeField] private string positiveFeedbackText;

    [TextArea]
    [SerializeField] private string negativeFeedbackText;



    private static UIController instance;
    public static UIController Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        timerTMP.text = $"0";
        canvasInstructions.SetActive(true);
    }

    public void DeactivateInstructions()
    {
        canvasInstructions.SetActive(false);
    }
    public void UpdateTimerTMP(int seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int remainingSeconds = Mathf.FloorToInt(seconds % 60);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);

        timerTMP.text = formattedTime;
    }

    public void GameOverUI(bool timesUp, int points)
    {
        canvasGameOver.SetActive(true);
        if (timesUp) feedbackTMP.text = positiveFeedbackText;
        else feedbackTMP.text = negativeFeedbackText;
        pointsTMP.text = $"Points: {points}";

    }
}

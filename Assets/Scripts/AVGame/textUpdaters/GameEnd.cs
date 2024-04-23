using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;


//todo clean code
public class GameEnd : MonoBehaviour
{
    
    public GameObject evaluationWindow;
    public GameObject mainCanvas;
    public SpriteRenderer backButton;
    public SpriteRenderer restartButton;
    public TextMeshProUGUI evaluationText;


    private void Awake()
    {   
        //DontDestroyOnLoad(evaluationWindow);
        DontDestroyOnLoad(mainCanvas);
        evaluationWindow.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.avFinished += CountdownFinished;
        LogStatisticsEvents.showPlayerStatistics += SetEvaluationText;
        GameManager.backButtonPressed += NullText;
        GameManager.restartButtonPressed += NullText;
    }

    private void OnDisable()
    {
        GameManager.avFinished -= CountdownFinished;
        LogStatisticsEvents.showPlayerStatistics -= SetEvaluationText;
        GameManager.backButtonPressed -= NullText;
        GameManager.restartButtonPressed -= NullText;
    }

    private void NullText()
    {
        evaluationText.text = "";
    }
    
    private void SetEvaluationText(DataToSave data)
    {
        evaluationText.text = "Time lasted: " + data.timeLasted + "s\n"
                              + "Max objects handeled: " + data.maxObjectCount + "\n"
                              + "For other stats see statistics";
    }
    private void Start()
    {
        evaluationWindow.SetActive(false);
    }

    private void CountdownFinished()
    {
        mainCanvas.SetActive(false);
        evaluationWindow.SetActive(true);
    }
}
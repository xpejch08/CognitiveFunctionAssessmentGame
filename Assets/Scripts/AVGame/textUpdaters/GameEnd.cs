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


    private void Awake()
    {   
        DontDestroyOnLoad(evaluationWindow);
        DontDestroyOnLoad(mainCanvas);
        DontDestroyOnLoad(backButton);
        DontDestroyOnLoad(restartButton);
        evaluationWindow.SetActive(false);
        GameManager.avFinished += CountdownFinished; ;
    }

    private void Start()
    {
        evaluationWindow.SetActive(false);
    }

    private void SetEvaluation()
    {
            restartButton.enabled = true;
            backButton.enabled = true;
    }

    private void CountdownFinished()
    {
        SetEvaluation();
        mainCanvas.SetActive(false);
        evaluationWindow.SetActive(true);
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;


//todo clean code
public class CountdownTimer : MonoBehaviour
{
    private int _countdownTime = 20;
    private int _firstLevelTime = 5;
    private int _countdownSubtract = 5;
    private int _nextLevelTime;
    private int _currentLevel = 1;
    private int _squareSum = 0;
    private int _triangleSum = 0;
    private int _circleSum = 0;
    private int _minTime = 5;
    private int _DesiredAmount = 10;
    private int _levelAddition = 10;
    
    private TextMeshProUGUI _countdownText;
    public GameObject evaluationWindow;
    public GameObject mainCanvas;
    public TextMeshProUGUI evaluationText;
    public TextMeshProUGUI DesiredAmountText;
    public SpriteRenderer backButton;
    public SpriteRenderer nextLevelButton;
    public SpriteRenderer restartButton;


    private void Awake()
    {
        GameManager.nextLevel += NextLevelClicked;
        GameManager.firstLevel += RestartClicked;
        GameManager.sendSumSquare += SetSquareSum;
        GameManager.sendSumTriangle += SetTriangleSum;
        GameManager.sendSumCircle += SetCircleSum;
    }

    private void Start()
    {
        _countdownText = GetComponent<TextMeshProUGUI>();
        evaluationWindow.SetActive(false);
        
        StartCoroutine(StartCountdown());
    }

    private void UpdateDesiredAmount()
    {
        _DesiredAmount += _levelAddition;
    }
    //todo clean code
    private IEnumerator StartCountdown()
    {
        UpdateDesiredAmount();
        DesiredAmountText.text = "Desired Amount: " + _DesiredAmount;
        int time = _countdownTime;
        _nextLevelTime = _countdownTime - _countdownSubtract;
        while (time > 0)
        {
            _countdownText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        _countdownText.text = "0";
        
        CountdownFinished();
    }

    private void SetSquareSum(int sum)
    {
           _squareSum = sum;
    }
    private void SetTriangleSum(int sum)
    {
        _triangleSum = sum;
    }
    private void SetCircleSum(int sum)
    {
        _circleSum = sum;
    }

    private void SetEvaluation()
    {
        int sum = _squareSum + _triangleSum + _circleSum;
        if(sum > _DesiredAmount)
        {
            evaluationText.text = "Try Again! \n your sum: " + sum;
            nextLevelButton.enabled = false;
            restartButton.enabled = true;
            backButton.enabled = true;
        }
        else
        {
            evaluationText.text = "Good Job! \n your sum: " + sum;
            nextLevelButton.enabled = true;
            restartButton.enabled = false;
            backButton.enabled = true;
        }
    }

    private void ResetDesiredAmount()
    {
        _DesiredAmount = 10;
    }
    private void CountdownFinished()
    {
        GameManager.LevelFinished();
        SetEvaluation();
        mainCanvas.SetActive(false);
        evaluationWindow.SetActive(true);
        NextLevelSetUp();
        bool passed = true;
        nextLevelButton.enabled = passed;
        restartButton.enabled = passed;
        backButton.enabled = passed;
    }

    private void NextLevelSetUp()
    { 
        _countdownTime = _nextLevelTime;
        _currentLevel++;
        if (_countdownTime < _minTime)
        {
            _countdownTime = _minTime;
        }   
    }
    
    private void FirstLevelSetUp()
    {   
        _countdownTime = _firstLevelTime;
        _currentLevel = 1;
    }
    
    private void NextLevelClicked()
    {
        mainCanvas.SetActive(true);
        NextLevelSetUp();
        evaluationWindow.SetActive(false);
        StartCoroutine(StartCountdown());
    }
    private void RestartClicked()
    {
        ResetDesiredAmount();
        mainCanvas.SetActive(true);
        FirstLevelSetUp();
        evaluationWindow.SetActive(false);
        StartCoroutine(StartCountdown());
    }
}
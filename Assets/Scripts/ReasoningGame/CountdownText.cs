using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;


//todo clean code
public class CountdownTimer : MonoBehaviour
{
    private int _countdownTime = 20;
    private int _firstLevelTime = 20;
    private int _countdownSubtract = 2;
    private int _nextLevelTime;
    private int _currentLevel = 1;
    private int _squareSum = 0;
    private int _triangleSum = 0;
    private int _circleSum = 0;
    private int _minTime = 5;
    private int _DesiredAmount = 10;
    private int _levelAddition = 10;
    private int _maxShapes = 8;
    private int ClickedCountSum = 0;
    private int _overshotCoeficient = 0;
    private List<int> _finalAmounts = new List<int>();
    private int _finalDeltaAverage;
    
    public DataToSaveReasoning dataToSaveReasoning = new DataToSaveReasoning();
    public TextMeshProUGUI CountdownText;
    public GameObject evaluationWindow;
    public GameObject mainCanvas;
    public TextMeshProUGUI evaluationText;
    public TextMeshProUGUI DesiredAmountText;
    public TextMeshProUGUI ShapesAvailibleText;
    public TextMeshProUGUI LevelText;
    public SpriteRenderer backButton;
    public SpriteRenderer nextLevelButton;
    public SpriteRenderer restartButton;
    private BoxCollider2D nextLevelCollider;

    private void OnEnable()
    {
        GameManager.nextLevel += NextLevelClicked;
        GameManager.firstLevel += RestartClicked;
        GameManager.sendSumSquare += SetSquareSum;
        GameManager.sendSumTriangle += SetTriangleSum;
        GameManager.sendSumCircle += SetCircleSum;
        MinMaxMidEvents.sendClickedCountCircle += CountClickedCountSum;
        MinMaxMidEvents.sendClickedCountTriangle += CountClickedCountSum;
        MinMaxMidEvents.sendClickedCountSquare += CountClickedCountSum;   
    }

    private void OnDisable()
    {
        GameManager.nextLevel -= NextLevelClicked;
        GameManager.firstLevel -= RestartClicked;
        GameManager.sendSumSquare -= SetSquareSum;
        GameManager.sendSumTriangle -= SetTriangleSum;
        GameManager.sendSumCircle -= SetCircleSum;
        MinMaxMidEvents.sendClickedCountCircle -= CountClickedCountSum;
        MinMaxMidEvents.sendClickedCountTriangle -= CountClickedCountSum;
        MinMaxMidEvents.sendClickedCountSquare -= CountClickedCountSum;
    }


    private void Start()
    {
        evaluationWindow.SetActive(false);
        InitialiseShapeAvailibleText();
        nextLevelCollider = nextLevelButton.GetComponent<BoxCollider2D>();
        StartCoroutine(StartCountdown());
        SetLevelText();
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
            CountdownText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        CountdownText.text = "0";
        
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
            evaluationText.text = "You overshot! \n your sum: " + sum + "\nFor more stats\nsee statistics";
        }
        else
        {
            evaluationText.text = "Good Job! \n your sum: " + sum + "\nFor more stats\nsee statistics";
        }
    }
    
    private void SetDataTOSaveReasoning()
    {
        dataToSaveReasoning.desiredAmount = _DesiredAmount;
        dataToSaveReasoning.finalAmount = _squareSum + _triangleSum + _circleSum;
        dataToSaveReasoning.level = _currentLevel;
    }

    private void ResetDesiredAmount()
    {
        _DesiredAmount = 10;
    }

    private void CalculateFinalDeltaAndAddToList()
    {
        int desiredFinalDelta = -(_DesiredAmount - (_squareSum + _triangleSum + _circleSum));
        _finalAmounts.Add(desiredFinalDelta);
    }

    private void CalculateAverageFinalDelta()
    {
        int allDeltas = 0;
        for (int i = 0; i < _finalAmounts.Count; i++)
        {
            allDeltas += _finalAmounts[i];
        }
        _finalDeltaAverage = allDeltas / _finalAmounts.Count;
    }
    private void CountdownFinished()
    {
        GameManager.LevelFinished();
        SetEvaluation();
        SetDataTOSaveReasoning();
        CalculateFinalDeltaAndAddToList();
        mainCanvas.SetActive(false);
        evaluationWindow.SetActive(true);
        
        bool passed = true;
        if (_currentLevel == 10)
        {   
            CalculateAverageFinalDelta();
            dataToSaveReasoning.desiredFinalDelta = _finalDeltaAverage;
            dataToSaveReasoning.overshotCoefficient = _overshotCoeficient;
            LogStatisticsEvents.SendPLayerStatisticsReasoning(dataToSaveReasoning);
            nextLevelButton.enabled = false;
            nextLevelCollider.enabled = false;
            restartButton.enabled = passed;
            backButton.enabled = passed;   
        }
        else
        {
            InitialiseShapeAvailibleText();
            nextLevelButton.enabled = passed;
            restartButton.enabled = passed;
            backButton.enabled = passed;   
        }
    }
    
    private void UpdateOverShotCoeficient()
    {
        int desiredFinalDelta = -(_DesiredAmount - (_squareSum + _triangleSum + _circleSum));
        if(desiredFinalDelta > 0)
        {
            _overshotCoeficient++;
        }
        else
        {
            _overshotCoeficient--;
        
        }
    }
    private void SetSumsToZero()
    {
        _squareSum = 0;
        _triangleSum = 0;
        _circleSum = 0;
    }
    private void NextLevelSetUp()
    { 
        _countdownTime = _nextLevelTime;
        _currentLevel++;
        InitialiseShapeAvailibleText();
        UpdateOverShotCoeficient();
        SetSumsToZero();
        ClickedCountSum = 0;
        if (_countdownTime < _minTime)
        {
            _countdownTime = _minTime;
        }   
        SetLevelText();
    }
    
    private void FirstLevelSetUp()
    {   
        ClickedCountSum = 0;
        nextLevelCollider.enabled = true;
        nextLevelButton.enabled = true;
        _countdownTime = _firstLevelTime;
        _currentLevel = 1;
        _overshotCoeficient = 0;
        SetLevelText();
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
        InitialiseShapeAvailibleText();
        mainCanvas.SetActive(true);
        FirstLevelSetUp();
        evaluationWindow.SetActive(false);
        StartCoroutine(StartCountdown());
    }

    private void SetAvailableShapesText(int shapes)
    {
        ShapesAvailibleText.text = "Shapes left: " + shapes;
    }
    
    private void SetLevelText()
    {
        LevelText.text = "Level: " + _currentLevel;
    }
    private void InitialiseShapeAvailibleText()
    {
        ShapesAvailibleText.text = "Shapes left: 8";
    }
    private void CountClickedCountSum(int clickedCount)
    {
        
       ClickedCountSum += clickedCount;
       SetAvailableShapesText(_maxShapes-ClickedCountSum);
       if (ClickedCountSum == 8)
       {
           MinMaxMidEvents.SendClickedCountSum(false);   
       }
       else
       {
           MinMaxMidEvents.SendClickedCountSum(true);
       }
    }
}
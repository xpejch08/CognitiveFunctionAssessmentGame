using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ReasoningSquare : MonoBehaviour
{
    private List<int> _potentialValues = new List<int>();
    private int _currentValue;
    private string _intervalDisplay;
    private int _firstValue = 3;
    private int _intervalSize = 4;
    private int _clickedCount = 0;
    private int _maxValue;
    private int _minValue;
    private int _midValue;
    private int _sum = 0;
    private SpriteRenderer _squareObject;
    
    
    private void AddTwoValuesFirst()
    {
        for (int i = 0; i < 2; i++)
        {
            _potentialValues.Add(_firstValue);
            _firstValue++;
        }
    }

    private void AddFourValues()
    {
        for (int i = 0; i < 4; i++)
        {
            _potentialValues.Add(_firstValue);
            _firstValue++;
        }
    }
    
    private void SubtractFirstTwoValues()
    {
        for (int i = 0; i < 2; i++)
        {
            _potentialValues.RemoveAt(0);
        }
    }

    private void GenerateSumWithRandomIntervalNumbers()
    {   
        for (int i = 0; i < _clickedCount; i++)
        {
            int randomIndex = Random.Range(0, _potentialValues.Count - 1); 
            _sum += _potentialValues[randomIndex];
        }
        GameManager.SendSumSquare(_sum);
    }
    
    private void CreateStringWithInterval()
    {
        _intervalDisplay = "";
        int numberOfValues = _potentialValues.Count;
        _intervalDisplay += _potentialValues[0] + " - ";
        _intervalDisplay += _potentialValues[numberOfValues - 1];
        
    }
    private void OnMouseDown()
    {
        _clickedCount++;
        string count = _clickedCount.ToString();
        GameManager.ChangeSquareCount(count);
        CountMinMaxMid();
        MinMaxMidEvents.SendMinMaxMidSquare(_minValue, _maxValue, _midValue);
    }
    private void SubtractCount()
    {
        _clickedCount--;
        if (_clickedCount < 0)
        {
            _clickedCount = 0;
        }
        string count = _clickedCount.ToString();
        GameManager.ChangeSquareCount(count);
        CountMinMaxMid();
        MinMaxMidEvents.SendMinMaxMidSquare(_minValue, _maxValue, _midValue);
    }

    private void CountMinMaxMid()
    {
        _maxValue = _potentialValues[_potentialValues.Count - 1] * _clickedCount;
        _midValue = _potentialValues[_potentialValues.Count / 2] * _clickedCount;
        _minValue = _potentialValues[0] * _clickedCount;
    }

    private void ResetCount()
    {
        _clickedCount = 0;
        string count = _clickedCount.ToString();
        GameManager.ChangeSquareCount(count);
    }
    private void ResetMinMaxMid()
    {
        _maxValue = 0;
        _midValue = 0;
        _minValue = 0;
        MinMaxMidEvents.SendMinMaxMidSquare(0,0,0);
    }
    private void NextLevelClicked()
    {   
        ResetMinMaxMid();
        ResetCount();
        _sum = 0;
        SubtractFirstTwoValues();
        AddFourValues();   
        CreateStringWithInterval();
        GameManager.ChangeTextSquare(_intervalDisplay);
    }
    
    private void RestartClicked()
    {
        ResetMinMaxMid();
        ResetCount();
        _potentialValues.Clear();
        _firstValue = 3;
        _intervalSize = 4;
        _sum = 0;
        AddTwoValuesFirst();
        CreateStringWithInterval();
        GameManager.ChangeTextSquare(_intervalDisplay);
    }
    void Start()
    {
        Random.InitState(DateTime.Now.Millisecond);
        AddTwoValuesFirst();
        CreateStringWithInterval();
        _squareObject = GetComponent<SpriteRenderer>();
        GameManager.nextLevel += NextLevelClicked;
        GameManager.firstLevel += RestartClicked;
        GameManager.squareSubtract += SubtractCount;
        GameManager.levelFinished += GenerateSumWithRandomIntervalNumbers;
        GameManager.ChangeTextSquare(_intervalDisplay);
        GameManager.ChangeSquareCount("0");
        MinMaxMidEvents.SendMinMaxMidSquare(0,0,0);
    }
    
    
}
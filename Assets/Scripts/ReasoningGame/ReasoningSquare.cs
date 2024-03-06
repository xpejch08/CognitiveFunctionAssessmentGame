using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ReasoningSquare : MonoBehaviour
{
    private List<int> _potentialValues = new List<int>();
    private int _currentValue;
    private string _intervalDisplay;
    private bool _addFour = true;
    private int _firstValue = 3;
    private int _intervalSize = 4;
    
    
    private void AddTwoValuesFirst()
    {
        for (int i = 0; i < 2; i++)
        {
            _potentialValues.Add(_firstValue);
            _firstValue++;
        }
    }

    private void AddFourValuesSecondIteration()
    {
        for (int i = 0; i < _intervalSize; i++)
        {
            _potentialValues.Add(_firstValue);
            _firstValue++;
        }
        _intervalSize += 2;
        _addFour = false;
    }
    private void AddNValuesNotFirst()
    {
        for (int i = 0; i < _intervalSize - 2; i++)
        {
            _potentialValues.Add(_firstValue);
            _firstValue++;
        }
        _intervalSize += 2;
    }
    
    private void SubtractFirstTwoValues()
    {
        for (int i = 0; i < 2; i++)
        {
            _potentialValues.RemoveAt(0);
        }
    }

    private void GenerateRandomValue()
    {
        int randomIndex = Random.Range(0, _potentialValues.Count - 1);
        _currentValue = _potentialValues[randomIndex];
    }
    
    private void CreateStringWithInterval()
    {
        _intervalDisplay = "";
        int numberOfValues = _potentialValues.Count;
        _intervalDisplay += _potentialValues[0] + " - ";
        _intervalDisplay += _potentialValues[numberOfValues - 1];
        
    }
    
    private void NextLevelClicked()
    {
        SubtractFirstTwoValues();
        if (_addFour)
        {
         AddFourValuesSecondIteration();   
        }
        else
        {
            AddNValuesNotFirst();
        }
        GenerateRandomValue();
        CreateStringWithInterval();
        GameManager.ChangeTextSquare(_intervalDisplay);
    }
    void Start()
    {
        AddTwoValuesFirst();
        GenerateRandomValue();
        CreateStringWithInterval();
        GameManager.nextLevel += NextLevelClicked;
        GameManager.ChangeTextSquare(_intervalDisplay);
    }
    
}
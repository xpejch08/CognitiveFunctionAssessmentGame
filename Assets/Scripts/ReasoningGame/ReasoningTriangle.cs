using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ReasoningTriangle : MonoBehaviour
{
    private List<int> _potentialValues = new List<int>();
    private int _currentValue;
    private string _intervalDisplay;
    private int _firstValue = 1;
    
    
    private void AddTwoValues()
    {
        for (int i = 0; i < 2; i++)
        {
            _potentialValues.Add(_firstValue);
            _firstValue++;
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
        AddTwoValues();
        GenerateRandomValue();
        CreateStringWithInterval();
        GameManager.ChangeText(_intervalDisplay);
    }
    void Start()
    {
        AddTwoValues();
        GenerateRandomValue();
        CreateStringWithInterval();
        GameManager.nextLevel += NextLevelClicked;
        GameManager.ChangeText(_intervalDisplay);
    }
    
}

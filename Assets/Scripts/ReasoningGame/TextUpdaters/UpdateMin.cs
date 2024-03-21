using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMin : MonoBehaviour
{
    private TextMeshProUGUI _textField;

    private int _minSum = 0;
    private int _maxSum = 0;
    private int _midSum = 0;
    
    private int _lastTriangleMin;
    private int _lastTriangleMax;
    private int _lastTriangleMid;
    private int _lastCircleMin;
    private int _lastCircleMax;
    private int _lastCircleMid;
    private int _lastSquareMin;
    private int _lastSquareMax; 
    private int _lastSquareMid;

    public void Awake()
    {
        _textField = GetComponent<TextMeshProUGUI>();
        MinMaxMidEvents.sendMinMaxMidSquare += UpdateSquare;
        MinMaxMidEvents.sendMinMaxMidTriangle += UpdateCircle;
        MinMaxMidEvents.sendMinMaxMidCircle += UpdateTriangle;
    }
    
    private void UpdateSquare(int min, int max, int mid)
    {
        _lastSquareMin = min;
        _lastSquareMax = max;
        _lastSquareMid = mid;
        UpdateSum();
    }
    private void UpdateCircle(int min, int max, int mid)
    {
        _lastCircleMin = min;
        _lastCircleMax = max;
        _lastCircleMid = mid;
        UpdateSum();
    }
    private void UpdateTriangle(int min, int max, int mid)
    {
        _lastTriangleMin = min;
        _lastTriangleMax = max;
        _lastTriangleMid = mid;
        UpdateSum();
    }
    private void UpdateSum()
    {
        _minSum = _lastTriangleMin + _lastCircleMin + _lastSquareMin;
        _maxSum = _lastTriangleMax + _lastCircleMax + _lastSquareMax;
        _midSum = _lastTriangleMid + _lastCircleMid + _lastSquareMid;
        UpdateText(_minSum, _maxSum, _midSum);
    }

    private void UpdateText(int min, int max, int mid)
    {
        if (min != null)
        {
            _textField.text = "Min: " + min + " Mid: " + mid + " Max: " + max;
        }
    }
}
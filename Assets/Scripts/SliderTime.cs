using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public Slider timeSlider;
    public float gameTime = 60f;

    private float timeRemaining;

    private void Awake()
    {
        GameManager.onShapeClicked += AddTimeToSlider;
        GameManager.shapeMissed += SubtractTimeFromSlider;
    }

    private void Start()
    {
        timeRemaining = gameTime;
        timeSlider.maxValue = gameTime;
        timeSlider.value = gameTime;
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeSlider.value = timeRemaining;
        }
        else
        {
            return;
        }
    }
    
    private void AddTimeToSlider()
    {
        timeRemaining += 10f;
        checkTimeOverflow();
        timeSlider.value = timeRemaining;
    }
    
    private void SubtractTimeFromSlider()
    {
        timeRemaining -= 5f;
        checkTimeOverflow();
        timeSlider.value = timeRemaining;
    }
    
    private void checkTimeOverflow()
    {
        if (timeRemaining > gameTime)
        {
            timeRemaining = gameTime;
        }
    }
}
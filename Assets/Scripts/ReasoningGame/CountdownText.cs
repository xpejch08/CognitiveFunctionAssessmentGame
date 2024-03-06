using System;
using UnityEngine;
using UnityEngine.UI; // Make sure to use TMPro if you're using TextMeshPro
using System.Collections;
using TMPro;


//todo clean code
public class CountdownTimer : MonoBehaviour
{
    private int _countdownTime = 15;
    private int _firstLevelTime = 15;
    private int _countdownSubtract = 5;
    private int _nextLevelTime;
    private int _currentLevel = 1;
    private int _minTime = 5;
    
    private TextMeshProUGUI _countdownText;
    public GameObject evaluationWindow;
    public TextMeshProUGUI evaluationText;
    
    public SpriteRenderer backButton;
    public SpriteRenderer nextLevelButton;
    public SpriteRenderer restartButton;


    private void Awake()
    {
        GameManager.nextLevel += NextLevelClicked;
        GameManager.firstLevel += RestartClicked;
    }

    private void Start()
    {
        _countdownText = GetComponent<TextMeshProUGUI>();
        evaluationWindow.SetActive(false);
        
        StartCoroutine(StartCountdown());
    }
    

    private IEnumerator StartCountdown()
    {
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

    private void CountdownFinished()
    {
        evaluationWindow.SetActive(true);
        NextLevelSetUp();
        bool passed = true;
        evaluationText.text = passed ? "Good Job!" : "Try Again!";
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
        NextLevelSetUp();
        evaluationWindow.SetActive(false);
        StartCoroutine(StartCountdown());
    }
    private void RestartClicked()
    {
        FirstLevelSetUp();
        evaluationWindow.SetActive(false);
        StartCoroutine(StartCountdown());
    }
}
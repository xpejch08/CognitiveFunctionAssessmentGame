using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleSquare : MonoBehaviour
{
    private const float CountdownInterval = 1f;
    private const float RandomAppearanceIntervalMax = 6f;
    private const float RandomAppearanceIntervalMin = 2f;
    private const float VisibleInterval = 3f;
    private const float InitialReactionTime = 1000f;
    private const float AddingSquaresTimer = 20f;
    private float _reactionTime = 0f;
    private float _timeShapeAppeared = 0f;
    private IEnumerator _currentRandomAppearanceCoroutine;
    private IEnumerator _currentHideSquareCoroutine;
    private SpriteRenderer _spriteRenderer;
    private DataToSave _dataToSave = new DataToSave();
    private bool Clickable = false;
    public AudioSource burst;
    


    private void Start()
    {
        GameManager.avFinished += SendDataToSave;
        _dataToSave.fastestReactionTimeSquares = InitialReactionTime;
        _dataToSave.shapeType = "square";
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(TurnOffOnCountdown(CountdownInterval));
        StartCoroutine(StartAfterCountdown());
    }
    
    private IEnumerator StartAfterCountdown()
    {
        yield return new WaitForSeconds(AddingSquaresTimer);
        Clickable = true;
        RandomAppearance();
    }

    private IEnumerator TurnOffOnCountdown(float countDownInterval)
    {
        yield return new WaitForSeconds(countDownInterval);
        ToggleVisibility();
    }
    
    private void RandomAppearance()
    {
        float randomInterval = UnityEngine.Random.Range(RandomAppearanceIntervalMin, RandomAppearanceIntervalMax);
        _currentRandomAppearanceCoroutine = AppearAfterRandomInterval(randomInterval);
        StartCoroutine(_currentRandomAppearanceCoroutine);
    }
    
    private IEnumerator AppearAfterRandomInterval(float delay)
    {   
        yield return new WaitForSeconds(delay);
        ToggleVisibility();
        _timeShapeAppeared = Time.time;
        _currentHideSquareCoroutine = HideSquareAfterVisibleDelay();
        StartCoroutine(_currentHideSquareCoroutine);
    }
    
    private IEnumerator HideSquareAfterVisibleDelay()
    {
        yield return new WaitForSeconds(VisibleInterval);
        ToggleVisibility();
        GameManager.ShapeMissed();
        RandomAppearance();
    }
    private void OnMouseDown()
    {
        if (IsVisible() && Clickable)
        {
            StopActiveCoroutine();
            burst.Play();
            ToggleVisibility();
            _reactionTime = Time.time - _timeShapeAppeared;
            UpdateReactionTime();
            RandomAppearance();
        }
    }
    
    private void SendDataToSave()
    {
        if (this == null || _spriteRenderer == null) return;
        LogStatisticsEvents.SendPLayerStatistics(_dataToSave);
        _dataToSave.fastestReactionTimeSquares = InitialReactionTime;
        _reactionTime = 0;
        Destroy(gameObject);
    }
    
    private void UpdateReactionTime()
    {
        if (_reactionTime < _dataToSave.fastestReactionTimeSquares)
        {
            _dataToSave.fastestReactionTimeSquares = _reactionTime;
        }
    }
    private void StopActiveCoroutine()
    {
        if (_currentRandomAppearanceCoroutine != null)
        {
            StopCoroutine(_currentRandomAppearanceCoroutine);
        }

        if (_currentHideSquareCoroutine != null)
        {
            StopCoroutine(_currentHideSquareCoroutine);
        }
    }
    private bool IsVisible()
    {
        return _spriteRenderer.enabled;
    }
    private void ToggleVisibility()
    {
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
    }
}
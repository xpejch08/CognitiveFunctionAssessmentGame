using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleTriangle : MonoBehaviour
{
    private const float CountdownInterval = 2f;
    private const float RandomAppearanceIntervalMax = 6f;
    private const float RandomAppearanceIntervalMin = 2f;
    private const float VisibleInterval = 3f;
    private const float InitialReactionTime = 1000f;
    private float _reactionTime = 0f;
    private float _timeShapeAppeared = 0f;
    private DataToSave _dataToSave = new DataToSave();
    private IEnumerator _currentRandomAppearanceCoroutine;
    private IEnumerator _currentHideTriangleCoroutine;
    private SpriteRenderer _spriteRenderer;
    private bool Clickable = false;
    public AudioSource burst;

    private void Start()
    {
        GameManager.avFinished += SendDataToSave;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _dataToSave.fastestReactionTimeTriangles = InitialReactionTime;
        _dataToSave.shapeType = "triangle";
        StartCoroutine(TurnOffOnCountdown(CountdownInterval));
        StartCoroutine(StartAfterCountdown());
    }
    
    private IEnumerator StartAfterCountdown()
    {
        yield return new WaitForSeconds(CountdownInterval);
        Clickable = true;
        RandomAppearance();
    }
    
    private void SendDataToSave()
    {
        if (this == null || _spriteRenderer == null) return;
        LogStatisticsEvents.SendPLayerStatistics(_dataToSave);
        _dataToSave.fastestReactionTimeTriangles = InitialReactionTime;
        _reactionTime = 0;
        Destroy(gameObject);
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
        ObjectCountEvents.ObjectAppeared();
        _timeShapeAppeared = Time.time;
        _currentHideTriangleCoroutine = HideTriangleAfterVisibleDelay();
        StartCoroutine(_currentHideTriangleCoroutine);
    }
    
    private IEnumerator HideTriangleAfterVisibleDelay()
    {
        yield return new WaitForSeconds(VisibleInterval);
        ToggleVisibility();
        ObjectCountEvents.ObjectDisappeared();
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
            ObjectCountEvents.ObjectDisappeared();
            _reactionTime = Time.time - _timeShapeAppeared;
            UpdateReactionTime();
            RandomAppearance();
        }
    }
    
    private void UpdateReactionTime()
    {
        if (_reactionTime < _dataToSave.fastestReactionTimeTriangles)
        {
            _dataToSave.fastestReactionTimeTriangles = _reactionTime;
        }
    }
    
    private void StopActiveCoroutine()
    {
        if (_currentRandomAppearanceCoroutine != null)
        {
            StopCoroutine(_currentRandomAppearanceCoroutine);
        }

        if (_currentHideTriangleCoroutine != null)
        {
            StopCoroutine(_currentHideTriangleCoroutine);
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
using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    private const float CountdownInterval = 4f;
    private const float RandomAppearanceIntervalMax = 15f;
    private const float RandomAppearanceIntervalMin = 10f;
    private const float VisibleInterval = 1f;
    private IEnumerator _currentRandomAppearanceCoroutine;
    private IEnumerator _currentHideTriangleCoroutine;
    private SpriteRenderer _spriteRenderer;
    private bool Clickable = false;

    private void Start()
    {
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(TurnOffOnCountdown(CountdownInterval));
        StartCoroutine(StartAfterCountdown());
    }
    
    private IEnumerator StartAfterCountdown()
    {
        yield return new WaitForSeconds(CountdownInterval);
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
        _currentHideTriangleCoroutine = HideTriangleAfterVisibleDelay();
        StartCoroutine(_currentHideTriangleCoroutine);
    }
    
    private IEnumerator HideTriangleAfterVisibleDelay()
    {
        yield return new WaitForSeconds(VisibleInterval);
        ToggleVisibility();
        RandomAppearance();
    }
    private void OnMouseDown()
    {
        if (IsVisible() && Clickable)
        {
            GameManager.ShapeClicked();
            StopActiveCoroutine();
            ToggleVisibility();
            RandomAppearance();
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
using System.Collections;
using UnityEngine;

public class TimedSprite : MonoBehaviour
{
    public AudioSource audioSource;

    public float minTimeBeforeBeep = 5f;
    public float maxTimeBeforeBeep = 10f;
    public float timeToAdd = 5f;

    private bool _canAddTime = false;

    void Start()
    {
        // Ensure the collider is set to be trigger to detect mouse clicks
        GetComponent<Collider2D>().isTrigger = true;

        // Start the initial beep routine
        StartBeepRoutine();
    }

    public void StartBeepRoutine()
    {
        StartCoroutine(BeepAndAllowPressRoutine());
    }

    private IEnumerator BeepAndAllowPressRoutine()
    {
        float randomInterval = UnityEngine.Random.Range(minTimeBeforeBeep, maxTimeBeforeBeep);
        yield return new WaitForSeconds(randomInterval);

        _canAddTime = true;
        audioSource.Play(); // Play beep sound

        yield return new WaitForSeconds(2); // Wait for 2 seconds to allow press

        _canAddTime = false;
        
        // Automatically restart the routine
        StartBeepRoutine();
    }

    private void OnMouseDown()
    {
        if (_canAddTime)
        {
            Debug.Log("Correct timing! Time added.");
            _canAddTime = false; // Reset the flag
            Handheld.Vibrate();
            
            StopAllCoroutines();
            StartBeepRoutine();
        }
        else
        {
            Debug.Log("Wrong timing!");
            GameManager.ShapeMissed();
            
            Handheld.Vibrate();
            StopAllCoroutines();
            StartBeepRoutine();
        }
    }
}

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
        GetComponent<Collider2D>().isTrigger = true;
        
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
        audioSource.Play();

        yield return new WaitForSeconds(2);

        _canAddTime = false;
        GameManager.ShapeMissed();
        
        StartBeepRoutine();
    }

    private void OnMouseDown()
    {
        if (_canAddTime)
        {
            Debug.Log("Correct timing! Time added.");
            _canAddTime = false;
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

using System.Collections;
using UnityEngine;

public class TimedSprite : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource ClickedAudioSource;

    public float minTimeBeforeBeep = 5f;
    public float maxTimeBeforeBeep = 10f;
    public float timeToAdd = 5f;
    
    private const float InitialReactionTime = 1000f;
    private float _reactionTime = 0f;
    private float _timeSoundPlayed = 0f;
    private DataToSave _dataToSave = new DataToSave();

    private bool _canAddTime = false;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        GameManager.avFinished += SendDataToSave;
        _dataToSave.fastestReactionTimeAudio = InitialReactionTime;
        _dataToSave.shapeType = "audio";
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
        _timeSoundPlayed = Time.time;
        yield return new WaitForSeconds(2);

        _canAddTime = false;
        GameManager.ShapeMissed();
        
        StartBeepRoutine();
    }

    public void OnMouseDown()
    {
        ClickedAudioSource.Play();
        if (_canAddTime)
        {
            Debug.Log("Correct timing! Time added.");
            _canAddTime = false;
            _reactionTime = Time.time - _timeSoundPlayed;
            UpdateReactionTime();
            StopAllCoroutines();
            StartBeepRoutine();
        }
        else
        {
            Debug.Log("Wrong timing!");
            GameManager.ShapeMissed();
            StopAllCoroutines();
            StartBeepRoutine();
        }
    }
    
    private void SendDataToSave()
    {
        if (this == null) return;
        LogStatisticsEvents.SendPLayerStatistics(_dataToSave);
        _dataToSave.fastestReactionTimeAudio = InitialReactionTime;
        _reactionTime = 0;
        Destroy(gameObject);
    }
    private void UpdateReactionTime()
    {
        if (_reactionTime < _dataToSave.fastestReactionTimeAudio)
        {
            _dataToSave.fastestReactionTimeAudio = _reactionTime;
        }
    }
}

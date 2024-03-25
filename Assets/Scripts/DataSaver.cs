using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Unity.VisualScripting;
using UnityEngine;




[Serializable]
public class DataToSave
{
    public string playerId;
    public float fastestReactionTimeSquares;
    public float fastestReactionTimeTriangles;
    public float fastestReactionTimeCircles;
    public float fastestReactionTimeDiamond;
    public float fastestReactionTimeAudio;
    public float timeLasted;
    public int maxObjectCount;
    public string shapeType;
}
public class DataSaver : MonoBehaviour
{
    private DatabaseReference reference;
    private const float _initialReactionTime = 1000f;
    private FirebaseAuth auth;
    private int _SquareCount = 6;
    private int _CircleCount = 6;
    private int _TriangleCount = 4;
    private int _DiamondCount = 1;
    private bool _maxObjectCountReached = false;
    private DataToSave _dataToSave = new DataToSave();

    void Awake() {
        _dataToSave.fastestReactionTimeSquares = _initialReactionTime;
        _dataToSave.fastestReactionTimeTriangles = _initialReactionTime;
        _dataToSave.fastestReactionTimeCircles = _initialReactionTime;
        _dataToSave.fastestReactionTimeDiamond = _initialReactionTime;
        _dataToSave.fastestReactionTimeAudio = _initialReactionTime;
        _dataToSave.maxObjectCount = 0;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
    }
    private void OnEnable()
    {
        LogStatisticsEvents.sendPlayerStatistics += SaveData;
    }
    private void OnDisable()
    {
        LogStatisticsEvents.sendPlayerStatistics -= SaveData;
    }
    
    //todo clean code
    private bool DecreaseSquareCount(DataToSave data)
    {
        if (data.shapeType == "square")
        {
            _SquareCount--;
            UpdateFastestReactionTime(data.fastestReactionTimeSquares, data.shapeType);
        } 
        if (data.shapeType == "triangle")
        {
            _TriangleCount--;
            UpdateFastestReactionTime(data.fastestReactionTimeTriangles, data.shapeType);
        } 
        if (data.shapeType == "circle")
        {
            _CircleCount--;
            UpdateFastestReactionTime(data.fastestReactionTimeCircles, data.shapeType);
        }

        if (data.shapeType == "diamond")
        {
            _DiamondCount--;
            UpdateFastestReactionTime(data.fastestReactionTimeDiamond, data.shapeType);
        }
        if(data.shapeType == "audio")
        {
            UpdateFastestReactionTime(data.fastestReactionTimeAudio, data.shapeType);
        }
        if (_SquareCount == 0 && _TriangleCount == 0 && _CircleCount == 0 && _DiamondCount == 0)
        {
            return false;
        }
        return true;
    }
    
    private void UpdateFastestReactionTime(float reactionTime, string shapeType)
    {
        if (shapeType == "square" && reactionTime < _dataToSave.fastestReactionTimeSquares)
        {
            _dataToSave.fastestReactionTimeSquares = reactionTime;
        }
        if (shapeType == "triangle" && reactionTime < _dataToSave.fastestReactionTimeTriangles)
        {
            _dataToSave.fastestReactionTimeTriangles = reactionTime;
        }
        if (shapeType == "circle" && reactionTime < _dataToSave.fastestReactionTimeCircles)
        {
            _dataToSave.fastestReactionTimeCircles = reactionTime;
        }
        if (shapeType == "diamond" && reactionTime < _dataToSave.fastestReactionTimeDiamond)
        {
            _dataToSave.fastestReactionTimeDiamond = reactionTime;
        }
        if (shapeType == "audio" && reactionTime < _dataToSave.fastestReactionTimeAudio)
        {
            _dataToSave.fastestReactionTimeAudio = reactionTime;
        }
    }
    
    private void CheckMaxObjectCount(DataToSave data)
    {
        if (data.maxObjectCount != 0)
        {
            _dataToSave.maxObjectCount = data.maxObjectCount;
            _maxObjectCountReached = true;
        }
    }
    private void SetTimeLasted(DataToSave data)
    {
        if (_dataToSave.timeLasted == 0)
        { 
            _dataToSave.timeLasted = data.timeLasted;   
        }
        
    }
    
    //todo clean
    public void SaveData(DataToSave data)
    {
        SetTimeLasted(data);
        CheckMaxObjectCount(data);
        if (DecreaseSquareCount(data))
        {
            return;
        }
        if (!_maxObjectCountReached)
        {
            return;
        }
        _dataToSave.playerId = data.playerId;
        if (auth.CurrentUser == null)
        {
            Debug.LogError("No authenticated user.");
            return;
        }
        else
        {   
            _dataToSave.playerId = auth.CurrentUser.UserId;
            string json = JsonUtility.ToJson(_dataToSave);
            reference.Child("users").Child(_dataToSave.playerId).Push().SetRawJsonValueAsync(json);   
        }
        ResetVariables();
    }

    private void ResetVariables()
    {
        _dataToSave.fastestReactionTimeSquares = _initialReactionTime;
        _dataToSave.fastestReactionTimeTriangles = _initialReactionTime;
        _dataToSave.fastestReactionTimeCircles = _initialReactionTime;
        _dataToSave.fastestReactionTimeDiamond = _initialReactionTime;
        _dataToSave.fastestReactionTimeAudio = _initialReactionTime;
        _dataToSave.timeLasted = 0;
        _dataToSave.maxObjectCount = 0;
        _SquareCount = 6;
        _CircleCount = 6;
        _TriangleCount = 4;
        _DiamondCount = 1;
        _maxObjectCountReached = false;
    }
}
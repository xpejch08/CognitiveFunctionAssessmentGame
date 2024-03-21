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
    private DataToSave _dataToSave = new DataToSave();

    void Awake() {
        _dataToSave.fastestReactionTimeSquares = _initialReactionTime;
        _dataToSave.fastestReactionTimeTriangles = _initialReactionTime;
        _dataToSave.fastestReactionTimeCircles = _initialReactionTime;
        LogStatisticsEvents.sendPlayerStatistics += SaveData;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
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
        if (_SquareCount == 0 && _TriangleCount == 0 && _CircleCount == 0)
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
    }
    
    public void SaveData(DataToSave data)
    {
        if (DecreaseSquareCount(data))
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
        _dataToSave.fastestReactionTimeSquares = _initialReactionTime;
        _dataToSave.fastestReactionTimeTriangles = _initialReactionTime;
        _dataToSave.fastestReactionTimeCircles = _initialReactionTime;
    }
}
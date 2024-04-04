using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Unity.VisualScripting;
using UnityEngine;



[Serializable]
public class DataToSaveReasoning
{
    public string playerId;
    public int desiredAmount;
    public int level;
    public int finalAmount;
    public int desiredFinalDelta;
    public string GameType = "Reasoning";
}
public class DataSaverReasoning : MonoBehaviour 
{
    private DatabaseReference reference;
    private FirebaseAuth auth;
    private DataToSaveReasoning _dataToSaveReasoning = new DataToSaveReasoning();

    private void Awake()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
    }

    private void OnEnable()
    {
        LogStatisticsEvents.sendPlayerStatisticsReasoning += SaveData;
    }

    private void OnDisable()
    {
        LogStatisticsEvents.sendPlayerStatisticsReasoning -= SaveData;
    }
    
    private void SaveData(DataToSaveReasoning data)
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("No authenticated user.");
            return;
        }
        else
        {   
            _dataToSaveReasoning = data;
            _dataToSaveReasoning.playerId = auth.CurrentUser.UserId;
            CountAndSetDelta();
            string json = JsonUtility.ToJson(_dataToSaveReasoning);
            reference.Child("users").Child(_dataToSaveReasoning.playerId).Push().SetRawJsonValueAsync(json);   
        }
        ResetData();
    }
    private void CountAndSetDelta()
    {
        _dataToSaveReasoning.desiredFinalDelta = -(_dataToSaveReasoning.desiredAmount-_dataToSaveReasoning.finalAmount);
    }

    private void ResetData()
    {
        _dataToSaveReasoning.desiredAmount = 0;
        _dataToSaveReasoning.level = 0;
        _dataToSaveReasoning.finalAmount = 0;
        _dataToSaveReasoning.desiredFinalDelta = 0;
    }
    
    
}
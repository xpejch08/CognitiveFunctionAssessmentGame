using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;


[Serializable]
public class DataToSave
{
    public string playerName;
    public int score;
    public int level;
    public int time;
}
public class DataSaver : MonoBehaviour
{
    public DataToSave dataToSave;
    public string userId;
    private DatabaseReference reference;

    void Awake() {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    
    public void SaveData()
    {
        string json = JsonUtility.ToJson(dataToSave);
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }
}
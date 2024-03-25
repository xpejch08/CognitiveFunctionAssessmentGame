using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Unity.VisualScripting;
using UnityEngine;

public class ReactionTimeLists
{
    public List<float> reactionTimesTriangles = new List<float>();
    public List<float> reactionTimesSquares = new List<float>();
    public List<float> reactionTimesCircles = new List<float>();
    public List<float> reactionTimesDiamonds = new List<float>();
    public List<float> reactionTimesAudio = new List<float>();
    public List<float> timeLasted = new List<float>();
    public List<float> maxObjectCount = new List<float>();
}

public class DataGetter : MonoBehaviour
{
    public ReactionTimeLists reactionTimeLists = new ReactionTimeLists();
    private DatabaseReference reference;
    private FirebaseAuth auth;
    public List<float> reactionTimesTriangles;
    public List<float> reactionTimesSquares;

    private void Awake()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        reactionTimesTriangles = new List<float>();
    }

    public void GetPlayerData()
    {
        string userId = auth.CurrentUser.UserId;
        reference.Child("users").Child(userId)
            .OrderByKey()
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    reactionTimesTriangles.Clear();
                    foreach (DataSnapshot childSnapshot in snapshot.Children)
                    {
                        DataToSave entry = JsonUtility.FromJson<DataToSave>(childSnapshot.GetRawJsonValue());
                        reactionTimeLists.reactionTimesTriangles.Add(entry.fastestReactionTimeTriangles);
                        reactionTimeLists.reactionTimesSquares.Add(entry.fastestReactionTimeSquares);
                        reactionTimeLists.reactionTimesCircles.Add(entry.fastestReactionTimeCircles);
                        reactionTimeLists.reactionTimesDiamonds.Add(entry.fastestReactionTimeDiamond);
                        reactionTimeLists.reactionTimesAudio.Add(entry.fastestReactionTimeAudio);
                        reactionTimeLists.timeLasted.Add(entry.timeLasted);
                        reactionTimeLists.maxObjectCount.Add(entry.maxObjectCount);
                    }
                    LogStatisticsEvents.DataRetrieved();
                }
            });
    }
}
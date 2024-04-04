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
    public Dictionary<string, ReactionTimeLists> allPlayersData = new Dictionary<string, ReactionTimeLists>();
    private DatabaseReference reference;
    private FirebaseAuth auth;
    public float averageTimeLastedOfPlayer;
    public List<float> timeLastedAllUsers;
    public float percentile;

    private void Awake()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        timeLastedAllUsers = new List<float>();
    }

    public void GetPlayerData(String type)
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
                    if (type == "triangles")
                    {
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            if (childSnapshot.HasChild("GameType") &&
                                childSnapshot.Child("GameType").Value.ToString() == "AV")
                            {
                                DataToSave entry = JsonUtility.FromJson<DataToSave>(childSnapshot.GetRawJsonValue());
                                reactionTimeLists.reactionTimesTriangles.Add(entry.fastestReactionTimeTriangles);
                            }
                        }   
                        LogStatisticsEvents.DataRetrievedTriangles();
                    }
                    else if (type == "squares")
                    {
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            if (childSnapshot.HasChild("GameType") &&
                                childSnapshot.Child("GameType").Value.ToString() == "AV")
                            {
                                DataToSave entry = JsonUtility.FromJson<DataToSave>(childSnapshot.GetRawJsonValue());
                                reactionTimeLists.reactionTimesSquares.Add(entry.fastestReactionTimeSquares);
                            }
                        }

                        LogStatisticsEvents.DataRetrievedSquares();
                    }
                    else if (type == "circles")
                    {
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            if (childSnapshot.HasChild("GameType") &&
                                childSnapshot.Child("GameType").Value.ToString() == "AV")
                            {
                                DataToSave entry = JsonUtility.FromJson<DataToSave>(childSnapshot.GetRawJsonValue());
                                reactionTimeLists.reactionTimesCircles.Add(entry.fastestReactionTimeCircles);
                            }
                        }   
                        LogStatisticsEvents.DataRetrievedCircles();
                    }
                    else if (type == "diamonds")
                    {
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            if (childSnapshot.HasChild("GameType") &&
                                childSnapshot.Child("GameType").Value.ToString() == "AV")
                            {
                                DataToSave entry = JsonUtility.FromJson<DataToSave>(childSnapshot.GetRawJsonValue());
                                reactionTimeLists.reactionTimesDiamonds.Add(entry.fastestReactionTimeDiamond);
                            }
                        }   
                        LogStatisticsEvents.DataRetrievedDiamonds();
                    }
                    else if (type == "audio")
                    {
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            if (childSnapshot.HasChild("GameType") &&
                                childSnapshot.Child("GameType").Value.ToString() == "AV")
                            {
                                DataToSave entry = JsonUtility.FromJson<DataToSave>(childSnapshot.GetRawJsonValue());
                                reactionTimeLists.reactionTimesAudio.Add(entry.fastestReactionTimeAudio);
                            }
                        }   
                        LogStatisticsEvents.DataRetrievedAudio();
                    }
                    else if (type == "timeLasted")
                    {
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            if (childSnapshot.HasChild("GameType") &&
                                childSnapshot.Child("GameType").Value.ToString() == "AV")
                            {
                                DataToSave entry = JsonUtility.FromJson<DataToSave>(childSnapshot.GetRawJsonValue());
                                reactionTimeLists.timeLasted.Add(entry.timeLasted);
                            }
                        }   
                        CountAverageOfTimeLasted();
                        LogStatisticsEvents.DataRetrievedTimeLasted();
                    }
                    else if (type == "maxObjectCount")
                    {
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            if (childSnapshot.HasChild("GameType") &&
                                childSnapshot.Child("GameType").Value.ToString() == "AV")
                            {
                                DataToSave entry = JsonUtility.FromJson<DataToSave>(childSnapshot.GetRawJsonValue());
                                reactionTimeLists.maxObjectCount.Add(entry.maxObjectCount);
                            }
                        }   
                        LogStatisticsEvents.DataRetrievedMaxObjectCount();
                    }
                }
            });
    }

    public void GetAllPlayerAverages()
    {
        reference.Child("users")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    timeLastedAllUsers.Clear();

                    foreach (DataSnapshot userSnapshot in snapshot.Children)
                    {
                        float total = 0;
                        int count = 0;

                        foreach (DataSnapshot childSnapshot in userSnapshot.Children)
                        {
                            if (childSnapshot.HasChild("GameType") &&
                                childSnapshot.Child("GameType").Value.ToString() == "AV")
                            {
                                DataToSave entry = JsonUtility.FromJson<DataToSave>(childSnapshot.GetRawJsonValue());
                                total += entry.timeLasted;
                                count++;
                            }
                        }

                        if (count > 0)
                        {
                            float average = total / count;
                            timeLastedAllUsers.Add(average); // Add the user's average time lasted to the list.
                        }
                    }
                    CalculateUserPercentile();
                    LogStatisticsEvents.AllDataRetrievedTimeLasted();
                }
            });
        
    }

    public void CountAverageOfTimeLasted()
    {
        float sum = 0;
        foreach (var time in reactionTimeLists.timeLasted)
        {
            sum = sum + time;
        }
        averageTimeLastedOfPlayer = sum / reactionTimeLists.timeLasted.Count;
    }

    public void CalculateUserPercentile()
    {
        int numUsersBelow = CalculateUsersBelowMyAverage();
        int totalUsers = timeLastedAllUsers.Count;
        if (numUsersBelow == 0)
        {
            percentile = 0;
            return;
        }
        percentile = (float)numUsersBelow / totalUsers * 100;
    }

    public int CalculateUsersBelowMyAverage()
    {
        int numUsersBelow = 0;
        timeLastedAllUsers.Sort();
        foreach (var average in timeLastedAllUsers)
        {
            if(average < averageTimeLastedOfPlayer)
            {
                numUsersBelow++;
            }
        }
        return numUsersBelow;
    }
    

}
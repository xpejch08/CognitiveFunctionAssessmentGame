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
}
public class DataSaverReasoning : MonoBehaviour 
{
        
}
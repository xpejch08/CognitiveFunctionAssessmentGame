using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MinMaxMidEvents : MonoBehaviour
{
    public static event UnityAction<int, int, int> sendMinMaxMidSquare;
    public static event UnityAction<int, int, int> sendMinMaxMidTriangle;
    public static event UnityAction<int, int, int> sendMinMaxMidCircle;
    
    public static void SendMinMaxMidSquare(int min, int max, int mid)
    {
        sendMinMaxMidSquare?.Invoke(min, max, mid);
    }
    
    public static void SendMinMaxMidTriangle(int min, int max, int mid)
    {
        sendMinMaxMidTriangle?.Invoke(min, max, mid);
    }

    public static void SendMinMaxMidCircle(int min, int max, int mid)
    {
        sendMinMaxMidCircle?.Invoke(min, max, mid);
    }
    

}

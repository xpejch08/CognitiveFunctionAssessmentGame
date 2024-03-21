using UnityEngine;
using UnityEngine.Events;

public class ObjectCountEvents : MonoBehaviour
{
    public static event UnityAction objectAppeared;
    public static event UnityAction objectDisappeared;

    public static void ObjectAppeared()
    {
        objectAppeared?.Invoke();
    }

    public static void ObjectDisappeared()
    {
        objectDisappeared?.Invoke();
    }
}
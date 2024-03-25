using UnityEngine;
using UnityEngine.Events;

public class LogStatisticsEvents : MonoBehaviour
{
    public static event UnityAction<DataToSave> sendPlayerStatistics;

    public static event UnityAction dataRetrieved;     
    
    
    public static void SendPLayerStatistics(DataToSave stats)
    {
        sendPlayerStatistics?.Invoke(stats);
    }
    public static void DataRetrieved()
    {
        dataRetrieved?.Invoke();
    }
    
}
using UnityEngine;
using UnityEngine.Events;

public class LogStatisticsEvents : MonoBehaviour
{
    public static event UnityAction<DataToSave> sendPlayerStatistics;     
    
    public static event UnityAction<DataToSave> sendPlayerStatisticsTriangle;     
    
    public static event UnityAction<DataToSave> sendPlayerStatisticsCircle;     
    
    public static void SendPLayerStatistics(DataToSave stats)
    {
        sendPlayerStatistics?.Invoke(stats);
    }
    public static void SendPLayerStatisticsTriangle(DataToSave stats)
    {
        sendPlayerStatisticsTriangle?.Invoke(stats);
    }
    public static void SendPLayerStatisticsCircle(DataToSave stats)
    {
        sendPlayerStatisticsCircle?.Invoke(stats);
    }
}
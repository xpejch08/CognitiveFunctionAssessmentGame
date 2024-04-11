using UnityEngine;
using UnityEngine.Events;

public class LogStatisticsEvents : MonoBehaviour
{
    public static event UnityAction<DataToSave> sendPlayerStatistics;
    
    public static event UnityAction<DataToSave> showPlayerStatistics;
    public static event UnityAction<DataToSaveReasoning> sendPlayerStatisticsReasoning;

    public static event UnityAction dataRetrievedTriangles;     
    public static event UnityAction dataRetrievedSquares;
    public static event UnityAction dataRetrievedCircles;
    public static event UnityAction dataRetrievedDiamonds;
    
    public static event UnityAction dataRetrievedAudio;
    public static event UnityAction dataRetrievedTimeLasted;
    public static event UnityAction dataRetrievedMaxObjectCount;
    public static event UnityAction dataRetrievedFinalAmount;
    public static event UnityAction dataRetrievedDesiredFinalDelta;
    public static event UnityAction allDataRetrievedTimeLasted;
    public static event UnityAction allDataRetrievedFinalDelta;
    
    
    
    public static void SendPLayerStatistics(DataToSave stats)
    {
        sendPlayerStatistics?.Invoke(stats);
    }
    public static void ShowPLayerStatistics(DataToSave stats)
    {
        showPlayerStatistics?.Invoke(stats);
    }
    public static void SendPLayerStatisticsReasoning(DataToSaveReasoning stats)
    {
        sendPlayerStatisticsReasoning?.Invoke(stats);
    }
    public static void DataRetrievedTriangles()
    {
        dataRetrievedTriangles?.Invoke();
    }
    
    public static void DataRetrievedSquares()
    {
        dataRetrievedSquares?.Invoke();
    }
    public static void DataRetrievedCircles()
    {
        dataRetrievedCircles?.Invoke();
    }
    public static void DataRetrievedDiamonds()
    {
        dataRetrievedDiamonds?.Invoke();
    }
    public static void DataRetrievedAudio()
    {
        dataRetrievedAudio?.Invoke();
    }
    public static void DataRetrievedTimeLasted()
    {
        dataRetrievedTimeLasted?.Invoke();
    }
    public static void DataRetrievedMaxObjectCount()
    {
        dataRetrievedMaxObjectCount?.Invoke();
    }
    public static void DataRetrievedFinalAmount()
    {
        dataRetrievedFinalAmount?.Invoke();
    }
    public static void DataRetrievedDesiredFinalDelta()
    {
        dataRetrievedDesiredFinalDelta?.Invoke();
    }
    
    public static void AllDataRetrievedTimeLasted()
    {
        allDataRetrievedTimeLasted?.Invoke();
    }
    public static void AllDataRetrievedFinalDelta()
    {
        allDataRetrievedFinalDelta?.Invoke();
    }
}
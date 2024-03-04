using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    
    public static event UnityAction onShapeClicked; 
    
    public static event UnityAction shapeMissed;
    
    public static event Action<GameState> OnGameStateChanged;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeGameState(GameState.MainMenu);
    }

    public void ChangeGameState(GameState state)
    {
        gameState = state;

        switch (state)
        {
            case GameState.MainMenu:
                break;
            case GameState.Stats:
                break;
            case GameState.AudioVisualGame:
                break;
            case GameState.ReasoningGame:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        OnGameStateChanged?.Invoke(state);
    }
    
    public static void ShapeClicked()
    {
        onShapeClicked?.Invoke();
    }
    
    public static void ShapeMissed()
    {
        shapeMissed?.Invoke();
    }
    
}

public enum GameState
{
    MainMenu,
    Stats,
    AudioVisualGame,
    ReasoningGame,
    Lose
}
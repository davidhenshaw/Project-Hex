using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionView : MonoBehaviour
{
    public GameObject winDisplay;
    public GameObject loseDisplay;

    void Start()
    {
        GameSession.InstanceChanged += () => SubscribeToEvents();
        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        GameSession
            .Instance
            .StateChanged += OnStateChange;
    }

    private void OnStateChange(GameState oldState, GameState newState)
    {
        switch(oldState)
        {
            case GameState.WIN:
                winDisplay.SetActive(false);
                break;
            case GameState.LOST:
                loseDisplay.SetActive(false);
                break;
        }

        switch(newState)
        {
            case GameState.WIN:
                winDisplay.SetActive(true);
                break;
            case GameState.LOST:
                loseDisplay.SetActive(true);
                break;
        }
    }
}

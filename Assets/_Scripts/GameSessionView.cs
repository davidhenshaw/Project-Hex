using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSessionView : MonoBehaviour
{
    public GameObject winDisplay;
    public GameObject loseDisplay;
    public TMP_Text levelTitle;

    private void Awake()
    {
        GameSession.InstanceChanged += () => SubscribeToEvents();
        SubscribeToEvents();

        levelTitle.text = SceneManager.GetActiveScene().name;
    }

    void SubscribeToEvents()
    {
        GameSession
            .Instance
            .StateChanged += OnStateChange;

        SceneManager.sceneLoaded += OnLevelLoad;
    }

    private void OnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.StartsWith("Level"))
        {
            levelTitle.text = scene.name;
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSession : Singleton<GameSession>
{
    private float _loadDelay = 1.2f;

    public bool IsPaused { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (willSelfDestruct)
            return;
    }

    private void OnEnable()
    {
        GameEvents.Instance.AllBeesDied.AddListener(OnPlayerDied);
    }

    private void OnDisable()
    {
        if(GameEvents.Instance)
            GameEvents.Instance.AllBeesDied.RemoveListener(OnPlayerDied);
    }

    private void Update()
    {
        HandleInput();
    }


    public void TogglePause()
    {
        if (IsPaused)
            Unpause();
        else
            Pause();
    }

    public void Unpause()
    {
        IsPaused = false;
        GameEvents.Instance.UnpauseTriggered?.Invoke();
    }

    public void Pause()
    {
        IsPaused = true;
        GameEvents.Instance.PauseTriggered?.Invoke();
    }

    void OnPlayerDied()
    {

    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneLoader.Instance.ReloadLevel();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}

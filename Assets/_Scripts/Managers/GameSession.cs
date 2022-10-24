using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSession : Singleton<GameSession>
{
    private float _loadDelay = 1.2f;

    bool IsPaused { get; set; }

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

    private void OnGoalReached()
    {
        StartCoroutine(LoadNextLevel_co());
    }

    private void OnPlayerDied()
    {
        StartCoroutine(ResetLevel_co());
    }

    IEnumerator ResetLevel_co()
    {
        yield return new WaitForSeconds(_loadDelay);

        SceneLoader.Instance.ReloadLevel();
    }

    IEnumerator LoadNextLevel_co()
    {
        yield return new WaitForSeconds(_loadDelay);
        SceneLoader.Instance.LoadNextLevel();
    }

    private void OnDestroy()
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
            GameEvents.Instance.TogglePause();
        }
    }
}

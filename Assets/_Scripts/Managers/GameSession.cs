using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSession : Singleton<GameSession>
{
    public event Action<GameState, GameState> StateChanged;

    [SerializeField] PlayerController player;
    GameState _currentState;

    private float _loadDelay = 1.2f;

    public GameState CurrentState {
        get => _currentState;
        set {
            var oldState = _currentState;
            _currentState = value;
            StateChanged?.Invoke(oldState, _currentState);
        }
    }

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
        GameEvents.Instance.AllBeesDied.RemoveListener(OnPlayerDied);
    }

    private void Start()
    {
        Reset();
        CurrentState = GameState.PLAY;
    }

    private void Update()
    {
        HandleInput();
    }

    private void OnGoalReached()
    {
        CurrentState = GameState.WIN;
        StartCoroutine(LoadNextLevel_co());
    }

    private void OnPlayerDied()
    {
        CurrentState = GameState.LOST;
        StartCoroutine(ResetLevel_co());
    }

    IEnumerator ResetLevel_co()
    {
        yield return new WaitForSeconds(_loadDelay);
        Reset();
        SceneLoader.Instance.ReloadLevel();
    }

    IEnumerator LoadNextLevel_co()
    {
        yield return new WaitForSeconds(_loadDelay);
        Reset();
        SceneLoader.Instance.LoadNextLevel();
    }

    private void Reset()
    {
        CurrentState = GameState.PLAY;
    }

    private void OnDestroy()
    {
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
            SceneLoader.Instance.ReloadLevel();
        }
    }
}

public enum GameState
{
    PLAY, LOST, WIN
}

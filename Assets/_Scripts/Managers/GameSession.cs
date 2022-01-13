using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSession : Singleton<GameSession>
{
    public event Action<GameState, GameState> StateChanged;

    [SerializeField] PlayerController player;
    Board board;
    GameState _currentState;

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
        Goal.GoalReached += OnGoalReached;
        player.Died += OnPlayerDied;
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
    }

    private void OnPlayerDied()
    {
        CurrentState = GameState.LOST;
        StartCoroutine(ResetLevel_co());
    }

    IEnumerator ResetLevel_co()
    {
        yield return new WaitForSeconds(1.2f);
        Reset();
        SceneLoader.Instance.ReloadLevel();
    }

    private void Reset()
    {
        CurrentState = GameState.PLAY;
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

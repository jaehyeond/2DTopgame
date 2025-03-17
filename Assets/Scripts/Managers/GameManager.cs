using System;
using System.Collections;
using UnityEngine;
using VContainer;

/// <summary>
/// 게임의 상태와 흐름을 관리하는 매니저
/// </summary>
public class GameManager : BaseManager
{
    /// <summary>
    /// 게임 상태
    /// </summary>
    public enum GameState
    {
        None,           // 초기 상태
        Loading,        // 로딩 중
        MainMenu,       // 메인 메뉴
        Playing,        // 게임 플레이 중
        Paused,         // 일시 정지
        GameOver,       // 게임 오버
        Victory         // 승리
    }

    /// <summary>
    /// 현재 게임 상태
    /// </summary>
    private GameState _currentState = GameState.None;
    public GameState CurrentState => _currentState;

    /// <summary>
    /// 게임 상태 변경 이벤트
    /// </summary>
    public event Action<GameState> OnGameStateChanged;

    /// <summary>
    /// 현재 플레이어 레벨
    /// </summary>
    private int _playerLevel = 1;
    public int PlayerLevel => _playerLevel;

    /// <summary>
    /// 현재 타워 층수
    /// </summary>
    private int _currentFloor = 1;
    public int CurrentFloor => _currentFloor;

    /// <summary>
    /// 게임 시작 시간
    /// </summary>
    private float _gameStartTime = 0f;
    public float GameTime => Time.time - _gameStartTime;

    /// <summary>
    /// 의존성 주입
    /// </summary>
    [Inject]
    public GameManager()
    {
    }

    /// <summary>
    /// 매니저 초기화 구현
    /// </summary>
    protected override IEnumerator OnInitialize()
    {
        Debug.Log($"<color=yellow>[{_name}] 초기화 중...</color>");

        // 초기 게임 상태 설정
        ChangeGameState(GameState.MainMenu);

        yield return null;
    }

    /// <summary>
    /// 매니저 종료 구현
    /// </summary>
    protected override IEnumerator OnTerminate()
    {
        Debug.Log($"<color=yellow>[{_name}] 종료 중...</color>");

        // 게임 상태 초기화
        ChangeGameState(GameState.None);

        yield return null;
    }

    /// <summary>
    /// 게임 상태 변경
    /// </summary>
    /// <param name="newState">새로운 게임 상태</param>
    public void ChangeGameState(GameState newState)
    {
        if (_currentState == newState)
            return;

        Debug.Log($"<color=blue>[{_name}] 게임 상태 변경: {_currentState} -> {newState}</color>");

        // 이전 상태 종료 처리
        ExitState(_currentState);

        // 상태 변경
        _currentState = newState;

        // 새로운 상태 시작 처리
        EnterState(_currentState);

        // 이벤트 발생
        OnGameStateChanged?.Invoke(_currentState);
    }

    /// <summary>
    /// 상태 진입 처리
    /// </summary>
    /// <param name="state">진입할 상태</param>
    private void EnterState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                // 메인 메뉴 진입 처리
                break;
            case GameState.Loading:
                // 로딩 진입 처리
                break;
            case GameState.Playing:
                // 게임 플레이 진입 처리
                _gameStartTime = Time.time;
                break;
            case GameState.Paused:
                // 일시 정지 진입 처리
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                // 게임 오버 진입 처리
                break;
            case GameState.Victory:
                // 승리 진입 처리
                break;
        }
    }

    /// <summary>
    /// 상태 종료 처리
    /// </summary>
    /// <param name="state">종료할 상태</param>
    private void ExitState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                // 메인 메뉴 종료 처리
                break;
            case GameState.Loading:
                // 로딩 종료 처리
                break;
            case GameState.Playing:
                // 게임 플레이 종료 처리
                break;
            case GameState.Paused:
                // 일시 정지 종료 처리
                Time.timeScale = 1f;
                break;
            case GameState.GameOver:
                // 게임 오버 종료 처리
                break;
            case GameState.Victory:
                // 승리 종료 처리
                break;
        }
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame()
    {
        if (_currentState == GameState.MainMenu || _currentState == GameState.GameOver || _currentState == GameState.Victory)
        {
            // 게임 초기화
            _playerLevel = 1;
            _currentFloor = 1;

            // 게임 상태 변경
            ChangeGameState(GameState.Playing);
        }
    }

    /// <summary>
    /// 게임 일시 정지
    /// </summary>
    public void PauseGame()
    {
        if (_currentState == GameState.Playing)
        {
            ChangeGameState(GameState.Paused);
        }
    }

    /// <summary>
    /// 게임 재개
    /// </summary>
    public void ResumeGame()
    {
        if (_currentState == GameState.Paused)
        {
            ChangeGameState(GameState.Playing);
        }
    }

    /// <summary>
    /// 게임 오버
    /// </summary>
    public void GameOver()
    {
        if (_currentState == GameState.Playing)
        {
            ChangeGameState(GameState.GameOver);
        }
    }

    /// <summary>
    /// 게임 승리
    /// </summary>
    public void Victory()
    {
        if (_currentState == GameState.Playing)
        {
            ChangeGameState(GameState.Victory);
        }
    }

    /// <summary>
    /// 메인 메뉴로 이동
    /// </summary>
    public void ReturnToMainMenu()
    {
        ChangeGameState(GameState.MainMenu);
    }

    /// <summary>
    /// 플레이어 레벨 업
    /// </summary>
    public void LevelUp()
    {
        _playerLevel++;
        Debug.Log($"<color=green>[{_name}] 플레이어 레벨 업: {_playerLevel}</color>");
    }

    /// <summary>
    /// 다음 층으로 이동
    /// </summary>
    public void NextFloor()
    {
        _currentFloor++;
        Debug.Log($"<color=green>[{_name}] 다음 층으로 이동: {_currentFloor}</color>");

        // 최대 층수에 도달하면 승리
        if (_currentFloor > Define.MAX_TOWER_FLOOR)
        {
            Victory();
        }
    }
} 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// 게임 진행 및 타워 진행 관리 시스템
/// </summary>
public class ProgressionSystem : BaseSystem
{
    /// <summary>
    /// 현재 타워 층
    /// </summary>
    private int _currentFloor = 0;
    public int CurrentFloor => _currentFloor;
    
    /// <summary>
    /// 최대 도달 층
    /// </summary>
    private int _maxReachedFloor = 0;
    public int MaxReachedFloor => _maxReachedFloor;
    
    /// <summary>
    /// 층 변경 이벤트
    /// </summary>
    public event Action<int> OnFloorChanged;
    
    /// <summary>
    /// 층 클리어 이벤트
    /// </summary>
    public event Action<int> OnFloorCleared;
    
    /// <summary>
    /// 게임 클리어 이벤트
    /// </summary>
    public event Action OnGameCleared;
    
    /// <summary>
    /// 씬 매니저
    /// </summary>
    private SceneManagerEx _sceneManager;
    
    /// <summary>
    /// 맵 시스템
    /// </summary>
    private MapSystem _mapSystem;
    
    /// <summary>
    /// 의존성 주입
    /// </summary>
    [Inject]
    public ProgressionSystem(SceneManagerEx sceneManager, MapSystem mapSystem)
    {
        _sceneManager = sceneManager;
        _mapSystem = mapSystem;
    }
    
    /// <summary>
    /// 시스템 초기화 구현
    /// </summary>
    protected override IEnumerator OnInitialize()
    {
        Debug.Log($"<color=yellow>[{_name}] 초기화 중...</color>");
        
        // 게임 데이터 로드
        LoadGameData();
        
        Debug.Log($"<color=green>[{_name}] 초기화 완료</color>");
        yield return null;
    }
    
    /// <summary>
    /// 시스템 종료 구현
    /// </summary>
    protected override IEnumerator OnTerminate()
    {
        Debug.Log($"<color=yellow>[{_name}] 종료 중...</color>");
        
        // 게임 데이터 저장
        SaveGameData();
        
        Debug.Log($"<color=red>[{_name}] 종료 완료</color>");
        yield return null;
    }
    
    /// <summary>
    /// 게임 데이터 로드
    /// </summary>
    private void LoadGameData()
    {
        // PlayerPrefs를 사용한 간단한 데이터 로드
        _maxReachedFloor = PlayerPrefs.GetInt("MaxReachedFloor", 0);
        
        Debug.Log($"<color=yellow>[{_name}] 게임 데이터 로드: 최대 도달 층 = {_maxReachedFloor}</color>");
    }
    
    /// <summary>
    /// 게임 데이터 저장
    /// </summary>
    private void SaveGameData()
    {
        // PlayerPrefs를 사용한 간단한 데이터 저장
        PlayerPrefs.SetInt("MaxReachedFloor", _maxReachedFloor);
        PlayerPrefs.Save();
        
        Debug.Log($"<color=yellow>[{_name}] 게임 데이터 저장: 최대 도달 층 = {_maxReachedFloor}</color>");
    }
    
    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame(int startFloor = 1)
    {
        // 시작 층 설정
        _currentFloor = Mathf.Clamp(startFloor, 1, _maxReachedFloor + 1);
        
        // 게임 씬으로 전환
        _sceneManager.LoadScene(Define.EScene.Game, true);
        
        Debug.Log($"<color=green>[{_name}] 게임 시작: 층 {_currentFloor}</color>");
    }
    
    /// <summary>
    /// 다음 층으로 이동
    /// </summary>
    public void GoToNextFloor()
    {
        // 현재 층 클리어 처리
        FloorCleared();
        
        // 다음 층으로 이동
        _currentFloor++;
        
        // 최대 도달 층 업데이트
        if (_currentFloor > _maxReachedFloor)
        {
            _maxReachedFloor = _currentFloor;
            SaveGameData();
        }
        
        // 층 변경 이벤트 발생
        OnFloorChanged?.Invoke(_currentFloor);
        
        // 맵 생성
        GenerateFloorMap();
        
        Debug.Log($"<color=green>[{_name}] 다음 층으로 이동: 층 {_currentFloor}</color>");
    }
    
    /// <summary>
    /// 이전 층으로 이동
    /// </summary>
    public void GoToPreviousFloor()
    {
        if (_currentFloor <= 1)
        {
            Debug.LogWarning($"[{_name}] 이미 첫 번째 층입니다.");
            return;
        }
        
        // 이전 층으로 이동
        _currentFloor--;
        
        // 층 변경 이벤트 발생
        OnFloorChanged?.Invoke(_currentFloor);
        
        // 맵 생성
        GenerateFloorMap();
        
        Debug.Log($"<color=yellow>[{_name}] 이전 층으로 이동: 층 {_currentFloor}</color>");
    }
    
    /// <summary>
    /// 특정 층으로 이동
    /// </summary>
    public void GoToFloor(int floor)
    {
        if (floor < 1 || floor > _maxReachedFloor)
        {
            Debug.LogWarning($"[{_name}] 유효하지 않은 층: {floor}");
            return;
        }
        
        // 층 설정
        _currentFloor = floor;
        
        // 층 변경 이벤트 발생
        OnFloorChanged?.Invoke(_currentFloor);
        
        // 맵 생성
        GenerateFloorMap();
        
        Debug.Log($"<color=yellow>[{_name}] 층 이동: 층 {_currentFloor}</color>");
    }
    
    /// <summary>
    /// 층 클리어 처리
    /// </summary>
    public void FloorCleared()
    {
        // 층 클리어 이벤트 발생
        OnFloorCleared?.Invoke(_currentFloor);
        
        Debug.Log($"<color=green>[{_name}] 층 클리어: 층 {_currentFloor}</color>");
        
        // 마지막 층 클리어 시 게임 클리어 처리
        if (_currentFloor >= Define.MAX_TOWER_FLOOR)
        {
            GameCleared();
        }
    }
    
    /// <summary>
    /// 게임 클리어 처리
    /// </summary>
    private void GameCleared()
    {
        // 게임 클리어 이벤트 발생
        OnGameCleared?.Invoke();
        
        Debug.Log($"<color=green>[{_name}] 게임 클리어!</color>");
    }
    
    /// <summary>
    /// 현재 층에 맞는 맵 생성
    /// </summary>
    private void GenerateFloorMap()
    {
        // 층에 따른 맵 타입 결정
        MapSystem.MapType mapType;
        
        if (_currentFloor % 10 == 0) // 10층마다 보스 맵
        {
            mapType = MapSystem.MapType.Boss;
        }
        else // 일반 층은 타워 맵
        {
            mapType = MapSystem.MapType.Tower;
        }
        
        // 맵 크기 설정 (층이 올라갈수록 약간씩 커짐)
        int mapSize = 50 + (_currentFloor / 10) * 5;
        
        // 맵 생성
        _mapSystem.GenerateMap(mapType, mapSize, mapSize);
        
        Debug.Log($"<color=yellow>[{_name}] 층 {_currentFloor} 맵 생성: 타입 = {mapType}, 크기 = {mapSize}x{mapSize}</color>");
    }
} 
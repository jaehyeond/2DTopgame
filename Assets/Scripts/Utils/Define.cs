using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 전체에서 사용되는 상수와 열거형을 정의하는 클래스
/// </summary>
public static class Define
{
    #region UI 관련 열거형
    /// <summary>
    /// UI 이벤트 타입
    /// </summary>
    public enum EUIEvent
    {
        Click,      // 클릭 이벤트
        PointerDown,// 포인터 다운 이벤트
        PointerUp,  // 포인터 업 이벤트
        Drag,       // 드래그 이벤트
    }
    #endregion

    #region 게임 상태 관련 열거형
    /// <summary>
    /// 씬 타입
    /// </summary>
    public enum EScene
    {
        Unknown,    // 알 수 없는 씬
        StartUp,    // 시작 씬
        MainMenu,   // 메인 메뉴 씬
        Game,       // 게임 씬
        Loading,    // 로딩 씬
    }

    /// <summary>
    /// 게임 상태
    /// </summary>
    public enum EGameState
    {
        Ready,      // 준비 상태
        Playing,    // 플레이 중
        Pause,      // 일시 정지
        GameOver,   // 게임 오버
        Clear,      // 클리어
    }

    /// <summary>
    /// 생물체 상태
    /// </summary>
    public enum ECreatureState
    {
        Idle,       // 대기 상태
        Moving,     // 이동 중
        Attacking,  // 공격 중
        Skill,      // 스킬 사용 중
        Dead,       // 사망
    }

    /// <summary>
    /// 생물체 타입
    /// </summary>
    public enum ECreatureType
    {
        None,       // 없음
        Player,     // 플레이어
        Monster,    // 몬스터
        NPC,        // NPC
    }
    #endregion

    #region 게임 레이어 관련 상수
    /// <summary>
    /// 게임 레이어
    /// </summary>
    public enum ELayer
    {
        Default     = 0,
        UI          = 5,
        Player      = 8,
        Monster     = 9,
        Ground      = 10,
        Item        = 11,
    }
    #endregion

    #region 게임 설정 관련 상수
    /// <summary>
    /// 게임 설정 관련 상수
    /// </summary>
    public const float DEFAULT_MOVE_SPEED = 5.0f;     // 기본 이동 속도
    public const float DEFAULT_ATTACK_RANGE = 2.0f;   // 기본 공격 범위
    public const float DEFAULT_ATTACK_DELAY = 1.0f;   // 기본 공격 딜레이
    public const int MAX_TOWER_FLOOR = 100;           // 최대 타워 층수
    #endregion
} 
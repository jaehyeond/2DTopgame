using System.Collections;
using UnityEngine;

/// <summary>
/// 게임 시스템 인터페이스
/// </summary>
public interface ISystem
{
    /// <summary>
    /// 시스템 초기화
    /// </summary>
    /// <returns>초기화 코루틴</returns>
    IEnumerator Initialize();

    /// <summary>
    /// 시스템 종료
    /// </summary>
    /// <returns>종료 코루틴</returns>
    IEnumerator Terminate();

    /// <summary>
    /// 시스템 일시 정지
    /// </summary>
    void Pause();

    /// <summary>
    /// 시스템 재개
    /// </summary>
    void Resume();
} 
using System.Collections;
using UnityEngine;

/// <summary>
/// 모든 매니저 클래스가 구현해야 하는 기본 인터페이스
/// </summary>
public interface IManager
{
    /// <summary>
    /// 매니저 초기화
    /// </summary>
    /// <returns>초기화 코루틴</returns>
    IEnumerator Initialize();

    /// <summary>
    /// 매니저 종료
    /// </summary>
    /// <returns>종료 코루틴</returns>
    IEnumerator Terminate();
} 
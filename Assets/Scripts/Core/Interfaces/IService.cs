using System.Collections;
using UnityEngine;

/// <summary>
/// 모든 서비스 클래스가 구현해야 하는 기본 인터페이스
/// </summary>
public interface IService
{
    /// <summary>
    /// 서비스 초기화
    /// </summary>
    /// <returns>초기화 코루틴</returns>
    IEnumerator Initialize();

    /// <summary>
    /// 서비스 종료
    /// </summary>
    /// <returns>종료 코루틴</returns>
    IEnumerator Terminate();
} 
using System.Collections;
using UnityEngine;

/// <summary>
/// 모든 서비스 클래스의 기본 클래스
/// </summary>
public abstract class BaseService : IService
{
    /// <summary>
    /// 서비스 이름
    /// </summary>
    protected string _name;

    /// <summary>
    /// 초기화 완료 여부
    /// </summary>
    protected bool _initialized = false;

    /// <summary>
    /// 생성자
    /// </summary>
    public BaseService()
    {
        _name = GetType().Name;
    }

    /// <summary>
    /// 서비스 초기화
    /// </summary>
    /// <returns>초기화 코루틴</returns>
    public virtual IEnumerator Initialize()
    {
        if (_initialized)
        {
            Debug.LogWarning($"[{_name}] 이미 초기화되었습니다.");
            yield break;
        }

        Debug.Log($"<color=yellow>[{_name}] 초기화 시작</color>");

        yield return OnInitialize();

        _initialized = true;
        Debug.Log($"<color=green>[{_name}] 초기화 완료</color>");
    }

    /// <summary>
    /// 서비스 초기화 구현
    /// </summary>
    /// <returns>초기화 코루틴</returns>
    protected abstract IEnumerator OnInitialize();

    /// <summary>
    /// 서비스 종료
    /// </summary>
    /// <returns>종료 코루틴</returns>
    public virtual IEnumerator Terminate()
    {
        if (!_initialized)
        {
            Debug.LogWarning($"[{_name}] 초기화되지 않았습니다.");
            yield break;
        }

        Debug.Log($"<color=yellow>[{_name}] 종료 시작</color>");

        yield return OnTerminate();

        _initialized = false;
        Debug.Log($"<color=red>[{_name}] 종료 완료</color>");
    }

    /// <summary>
    /// 서비스 종료 구현
    /// </summary>
    /// <returns>종료 코루틴</returns>
    protected abstract IEnumerator OnTerminate();
} 
using System.Collections;
using UnityEngine;

/// <summary>
/// 게임 시스템 기본 클래스
/// </summary>
public abstract class BaseSystem : ISystem
{
    /// <summary>
    /// 시스템 이름
    /// </summary>
    protected string _name;

    /// <summary>
    /// 초기화 여부
    /// </summary>
    protected bool _initialized = false;

    /// <summary>
    /// 일시 정지 여부
    /// </summary>
    protected bool _paused = false;

    /// <summary>
    /// 생성자
    /// </summary>
    public BaseSystem()
    {
        _name = GetType().Name;
    }

    /// <summary>
    /// 시스템 초기화
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
    /// 시스템 초기화 구현
    /// </summary>
    /// <returns>초기화 코루틴</returns>
    protected abstract IEnumerator OnInitialize();

    /// <summary>
    /// 시스템 종료
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
    /// 시스템 종료 구현
    /// </summary>
    /// <returns>종료 코루틴</returns>
    protected abstract IEnumerator OnTerminate();

    /// <summary>
    /// 시스템 일시 정지
    /// </summary>
    public virtual void Pause()
    {
        if (!_initialized)
        {
            Debug.LogWarning($"[{_name}] 초기화되지 않았습니다.");
            return;
        }

        if (_paused)
        {
            Debug.LogWarning($"[{_name}] 이미 일시 정지되었습니다.");
            return;
        }

        _paused = true;
        OnPause();
        Debug.Log($"<color=blue>[{_name}] 일시 정지</color>");
    }

    /// <summary>
    /// 시스템 일시 정지 구현
    /// </summary>
    protected virtual void OnPause() { }

    /// <summary>
    /// 시스템 재개
    /// </summary>
    public virtual void Resume()
    {
        if (!_initialized)
        {
            Debug.LogWarning($"[{_name}] 초기화되지 않았습니다.");
            return;
        }

        if (!_paused)
        {
            Debug.LogWarning($"[{_name}] 일시 정지되지 않았습니다.");
            return;
        }

        _paused = false;
        OnResume();
        Debug.Log($"<color=blue>[{_name}] 재개</color>");
    }

    /// <summary>
    /// 시스템 재개 구현
    /// </summary>
    protected virtual void OnResume() { }
} 
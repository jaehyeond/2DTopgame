using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 초기화 가능한 MonoBehaviour의 기본 클래스
/// </summary>
public class InitBase : MonoBehaviour
{
    /// <summary>
    /// 초기화 완료 여부
    /// </summary>
    protected bool _init = false;

    /// <summary>
    /// 초기화 메서드. 한 번만 실행되도록 보장합니다.
    /// </summary>
    /// <returns>초기화 성공 여부</returns>
    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    /// <summary>
    /// Awake 시점에 자동으로 초기화합니다.
    /// </summary>
    private void Awake()
    {
        Init();
    }
} 
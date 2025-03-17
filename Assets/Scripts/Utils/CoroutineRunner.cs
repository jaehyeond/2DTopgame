using System.Collections;
using UnityEngine;

/// <summary>
/// 코루틴 실행을 위한 MonoBehaviour 래퍼 클래스
/// VContainer를 사용하는 비-MonoBehaviour 클래스에서 코루틴을 실행할 수 있게 해줍니다.
/// </summary>
public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("@CoroutineRunner");
                _instance = go.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    /// <summary>
    /// 코루틴 시작
    /// </summary>
    public Coroutine RunCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }

    /// <summary>
    /// 코루틴 중지
    /// </summary>
    public void StopCoroutineWrapper(Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
} 
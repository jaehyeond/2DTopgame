using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 기존 클래스에 확장 메서드를 추가하는 클래스
/// </summary>
public static class Extension
{
    /// <summary>
    /// GameObject에 UI 이벤트를 바인딩합니다.
    /// </summary>
    /// <param name="go">대상 GameObject</param>
    /// <param name="action">이벤트 발생 시 실행할 액션</param>
    /// <param name="type">UI 이벤트 타입</param>
    public static void BindEvent(this GameObject go, Action<PointerEventData> action = null, Define.EUIEvent type = Define.EUIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    /// <summary>
    /// GameObject가 유효한지 확인합니다. (null이 아니고 활성화되어 있는지)
    /// </summary>
    /// <param name="go">확인할 GameObject</param>
    /// <returns>유효 여부</returns>
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    /// <summary>
    /// Transform의 모든 자식을 제거합니다.
    /// </summary>
    /// <param name="transform">대상 Transform</param>
    public static void DestroyAllChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// 리스트의 요소를 무작위로 섞습니다.
    /// </summary>
    /// <typeparam name="T">리스트 요소 타입</typeparam>
    /// <param name="list">섞을 리스트</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]); // swap
        }
    }

    /// <summary>
    /// 컴포넌트가 있으면 가져오고, 없으면 추가합니다.
    /// </summary>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    /// <param name="go">대상 GameObject</param>
    /// <returns>컴포넌트</returns>
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        return Utils.GetOrAddComponent<T>(go);
    }

    /// <summary>
    /// RectTransform을 화면 전체 크기로 설정합니다.
    /// </summary>
    /// <param name="rect">대상 RectTransform</param>
    public static void SetFullScreen(this RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
    }
} 
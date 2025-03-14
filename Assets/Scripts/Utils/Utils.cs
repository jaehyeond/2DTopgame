using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 게임 전체에서 사용되는 유틸리티 함수를 제공하는 클래스
/// </summary>
public static class Utils
{
    #region 컴포넌트 관리
    /// <summary>
    /// 게임 오브젝트에 컴포넌트가 없으면 추가하고, 있으면 가져옵니다.
    /// </summary>
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        if (go == null)
        {
            Debug.LogWarning($"[Utils] GetOrAddComponent: GameObject가 null입니다.");
            return null;
        }

        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }
    #endregion

    #region 자식 오브젝트 찾기
    /// <summary>
    /// 특정 이름의 자식 게임오브젝트를 찾습니다.
    /// </summary>
    /// <param name="go">부모 GameObject</param>
    /// <param name="name">찾을 자식의 이름 (null이면 모든 자식)</param>
    /// <param name="recursive">하위 계층까지 재귀적으로 검색할지 여부</param>
    /// <returns>찾은 자식 GameObject, 없으면 null</returns>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    /// <summary>
    /// 특정 이름의 자식에서 컴포넌트를 찾습니다.
    /// </summary>
    /// <typeparam name="T">찾을 컴포넌트 타입</typeparam>
    /// <param name="go">부모 GameObject</param>
    /// <param name="name">찾을 자식의 이름 (null이면 모든 자식)</param>
    /// <param name="recursive">하위 계층까지 재귀적으로 검색할지 여부</param>
    /// <returns>찾은 컴포넌트, 없으면 null</returns>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            Debug.LogWarning($"[Utils] FindChild: GameObject가 null입니다.");
            return null;
        }

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    /// <summary>
    /// 깊이에 상관없이 특정 이름의 하위 객체를 찾습니다.
    /// </summary>
    /// <param name="parent">부모 GameObject</param>
    /// <param name="childName">찾을 하위 객체의 이름</param>
    /// <param name="includeInactive">비활성화된 객체도 포함할지 여부</param>
    /// <returns>찾은 첫 번째 하위 객체, 없으면 null</returns>
    public static GameObject FindChildDeep(GameObject parent, string childName, bool includeInactive = true)
    {
        if (parent == null || string.IsNullOrEmpty(childName))
        {
            Debug.LogWarning($"[Utils] FindChildDeep: 부모 객체가 null이거나 자식 이름이 비어있습니다.");
            return null;
        }

        // 먼저 직접적인 자식 확인 (성능 최적화)
        Transform directChild = parent.transform.Find(childName);
        if (directChild != null)
        {
            return directChild.gameObject;
        }

        // 모든 하위 객체 검색
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(includeInactive);
        foreach (Transform child in allChildren)
        {
            if (child.name == childName && child.gameObject != parent)
            {
                return child.gameObject;
            }
        }

        Debug.LogWarning($"[Utils] FindChildDeep: '{childName}' 이름의 하위 객체를 찾을 수 없습니다.");
        return null;
    }
    #endregion

    #region UI 이벤트 관리
    /// <summary>
    /// 버튼에 클릭 이벤트를 추가합니다.
    /// </summary>
    /// <param name="button">대상 버튼</param>
    /// <param name="action">클릭 시 실행할 액션</param>
    /// <param name="buttonName">버튼 이름 (로그용, null이면 버튼 오브젝트 이름 사용)</param>
    public static void AddButtonClickEvent(Button button, UnityEngine.Events.UnityAction action, string buttonName = null)
    {
        if (button != null)
        {
            button.onClick.AddListener(action);
            Debug.Log($"<color=green>[Utils] {buttonName ?? button.name} 버튼 이벤트 등록 완료</color>");
        }
        else
        {
            Debug.LogError($"<color=red>[Utils] {buttonName ?? "지정된"} 버튼을 찾을 수 없습니다!</color>");
        }
    }
    #endregion

    #region 기타 유틸리티
    /// <summary>
    /// 문자열을 열거형으로 변환합니다.
    /// </summary>
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    /// <summary>
    /// 16진수 문자열을 Color로 변환합니다.
    /// </summary>
    public static Color HexToColor(string color)
    {
        if (color.Contains("#") == false)
            color = $"#{color}";

        ColorUtility.TryParseHtmlString(color, out Color parsedColor);

        return parsedColor;
    }
    #endregion
} 
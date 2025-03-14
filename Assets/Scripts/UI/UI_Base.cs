using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 모든 UI 컴포넌트의 기본 클래스
/// </summary>
public class UI_Base : InitBase
{
    /// <summary>
    /// UI 이벤트 핸들러 컴포넌트
    /// </summary>
    protected class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        /// <summary>
        /// 이벤트 콜백 액션
        /// </summary>
        public Action<PointerEventData> OnClickHandler = null;
        public Action<PointerEventData> OnDownHandler = null;
        public Action<PointerEventData> OnUpHandler = null;
        public Action<PointerEventData> OnDragHandler = null;

        /// <summary>
        /// 클릭 이벤트 처리
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickHandler?.Invoke(eventData);
        }

        /// <summary>
        /// 포인터 다운 이벤트 처리
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            OnDownHandler?.Invoke(eventData);
        }

        /// <summary>
        /// 포인터 업 이벤트 처리
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            OnUpHandler?.Invoke(eventData);
        }

        /// <summary>
        /// 드래그 이벤트 처리
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            OnDragHandler?.Invoke(eventData);
        }
    }

    /// <summary>
    /// GameObject에 UI 이벤트를 바인딩합니다.
    /// </summary>
    /// <param name="go">대상 GameObject</param>
    /// <param name="action">이벤트 발생 시 실행할 액션</param>
    /// <param name="type">UI 이벤트 타입</param>
    public static void BindEvent(GameObject go, Action<PointerEventData> action = null, Define.EUIEvent type = Define.EUIEvent.Click)
    {
        if (go == null)
        {
            Debug.LogWarning($"[UI_Base] BindEvent: GameObject가 null입니다.");
            return;
        }

        UI_EventHandler eventHandler = Utils.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.EUIEvent.Click:
                eventHandler.OnClickHandler = action;
                break;
            case Define.EUIEvent.PointerDown:
                eventHandler.OnDownHandler = action;
                break;
            case Define.EUIEvent.PointerUp:
                eventHandler.OnUpHandler = action;
                break;
            case Define.EUIEvent.Drag:
                eventHandler.OnDragHandler = action;
                break;
            default:
                Debug.LogWarning($"[UI_Base] BindEvent: 지원하지 않는 이벤트 타입입니다: {type}");
                break;
        }
    }

    /// <summary>
    /// 초기화 메서드
    /// </summary>
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }
} 
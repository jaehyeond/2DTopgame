using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using VContainer;
using VContainer.Unity;

/// <summary>
/// 팝업 UI의 기본 클래스
/// </summary>
public class UI_Popup : UI_Base
{
    /// <summary>
    /// 팝업 정렬 순서
    /// </summary>
    private int _sortingOrder = 10;

    /// <summary>
    /// 초기화 메서드
    /// </summary>
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // 캔버스 설정
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = _sortingOrder;
        }

        // 배경 이미지 설정
        Transform background = transform.Find("Background");
        if (background != null)
        {
            // 배경 클릭 이벤트 설정
            background.gameObject.BindEvent(OnBackgroundClick);
        }

        // 닫기 버튼 설정
        Button closeButton = Utils.FindChild<Button>(gameObject, "CloseButton");
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
        }

        return true;
    }

    /// <summary>
    /// 정렬 순서 설정
    /// </summary>
    /// <param name="sortingOrder">정렬 순서</param>
    public void SetSortingOrder(int sortingOrder)
    {
        _sortingOrder = sortingOrder;

        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = _sortingOrder;
        }
    }

    /// <summary>
    /// 배경 클릭 이벤트
    /// </summary>
    /// <param name="data">이벤트 데이터</param>
    protected virtual void OnBackgroundClick(UnityEngine.EventSystems.PointerEventData data)
    {
        // 기본적으로 배경 클릭 시 팝업 닫기
        // 필요에 따라 자식 클래스에서 오버라이드
        Close();
    }

    /// <summary>
    /// 팝업 닫기
    /// </summary>
    public virtual void Close()
    {
        // VContainer를 통해 UIManager 가져오기
        if (ModularApplicationController.Instance != null)
        {
            var container = ModularApplicationController.Instance.Container;
            if (container != null)
            {
                var uiManager = container.Resolve<UIManager>();
                if (uiManager != null)
                {
                    uiManager.ClosePopupUI(this);
                    return;
                }
            }
        }
        
        Debug.LogWarning("[UI_Popup] UIManager를 찾을 수 없습니다.");
        Destroy(gameObject);
    }
} 
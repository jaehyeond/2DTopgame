using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬 UI의 기본 클래스
/// </summary>
public class UI_Scene : UI_Base
{
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
            canvas.sortingOrder = 0;
        }

        return true;
    }
} 
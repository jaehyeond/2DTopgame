using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// UI 화면 및 요소를 관리하는 매니저
/// </summary>
public class UIManager : BaseManager
{
    /// <summary>
    /// UI 루트 오브젝트
    /// </summary>
    private GameObject _root;

    /// <summary>
    /// 씬 UI 루트 오브젝트
    /// </summary>
    private GameObject _sceneUIRoot;

    /// <summary>
    /// 팝업 UI 루트 오브젝트
    /// </summary>
    private GameObject _popupUIRoot;

    /// <summary>
    /// 월드 스페이스 UI 루트 오브젝트
    /// </summary>
    private GameObject _worldSpaceUIRoot;

    /// <summary>
    /// 현재 열려있는 팝업 스택
    /// </summary>
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

    /// <summary>
    /// 현재 씬 UI
    /// </summary>
    private UI_Scene _sceneUI;

    /// <summary>
    /// 리소스 매니저
    /// </summary>
    private ResourceManager _resourceManager;

    /// <summary>
    /// 의존성 주입
    /// </summary>
    [Inject]
    public UIManager(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    /// <summary>
    /// 매니저 초기화 구현
    /// </summary>
    protected override IEnumerator OnInitialize()
    {
        Debug.Log($"<color=yellow>[{_name}] 초기화 중...</color>");

        // UI 루트 오브젝트 생성
        _root = new GameObject("@UI_Root");
        Object.DontDestroyOnLoad(_root);

        // 씬 UI 루트 오브젝트 생성
        _sceneUIRoot = new GameObject("SceneUI");
        _sceneUIRoot.transform.SetParent(_root.transform);

        // 팝업 UI 루트 오브젝트 생성
        _popupUIRoot = new GameObject("PopupUI");
        _popupUIRoot.transform.SetParent(_root.transform);

        // 월드 스페이스 UI 루트 오브젝트 생성
        _worldSpaceUIRoot = new GameObject("WorldSpaceUI");
        _worldSpaceUIRoot.transform.SetParent(_root.transform);

        yield return null;
    }

    /// <summary>
    /// 매니저 종료 구현
    /// </summary>
    protected override IEnumerator OnTerminate()
    {
        Debug.Log($"<color=yellow>[{_name}] 종료 중...</color>");

        // 모든 UI 정리
        ClearAllUI();

        yield return null;
    }

    /// <summary>
    /// 모든 UI 정리
    /// </summary>
    public void ClearAllUI()
    {
        // 모든 팝업 닫기
        CloseAllPopupUI();

        // 씬 UI 제거
        if (_sceneUI != null)
        {
            Object.Destroy(_sceneUI.gameObject);
            _sceneUI = null;
        }

        // 월드 스페이스 UI 제거
        if (_worldSpaceUIRoot != null)
        {
            foreach (Transform child in _worldSpaceUIRoot.transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }

    /// <summary>
    /// 씬 UI 설정
    /// </summary>
    /// <typeparam name="T">씬 UI 타입</typeparam>
    /// <param name="name">리소스 이름</param>
    /// <returns>생성된 씬 UI</returns>
    public T SetSceneUI<T>(string name = null) where T : UI_Scene
    {
        // 기존 씬 UI 제거
        if (_sceneUI != null)
        {
            Object.Destroy(_sceneUI.gameObject);
            _sceneUI = null;
        }

        // 리소스 이름이 없으면 타입 이름 사용
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        // 씬 UI 생성
        GameObject go = _resourceManager.Instantiate($"UI/Scene/{name}");
        if (go == null)
        {
            Debug.LogError($"[{_name}] SetSceneUI: 씬 UI를 찾을 수 없습니다. 경로: UI/Scene/{name}");
            return null;
        }

        // 부모 설정
        go.transform.SetParent(_sceneUIRoot.transform);

        // 위치 및 스케일 초기화
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        // 컴포넌트 가져오기
        T sceneUI = Utils.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        return sceneUI;
    }

    /// <summary>
    /// 팝업 UI 생성
    /// </summary>
    /// <typeparam name="T">팝업 UI 타입</typeparam>
    /// <param name="name">리소스 이름</param>
    /// <returns>생성된 팝업 UI</returns>
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // 리소스 이름이 없으면 타입 이름 사용
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        // 팝업 UI 생성
        GameObject go = _resourceManager.Instantiate($"UI/Popup/{name}");
        if (go == null)
        {
            Debug.LogError($"[{_name}] ShowPopupUI: 팝업 UI를 찾을 수 없습니다. 경로: UI/Popup/{name}");
            return null;
        }

        // 부모 설정
        go.transform.SetParent(_popupUIRoot.transform);

        // 위치 및 스케일 초기화
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        // 컴포넌트 가져오기
        T popup = Utils.GetOrAddComponent<T>(go);

        // 팝업 스택에 추가
        _popupStack.Push(popup);

        return popup;
    }

    /// <summary>
    /// 팝업 UI 닫기
    /// </summary>
    /// <param name="popup">닫을 팝업 UI</param>
    public void ClosePopupUI(UI_Popup popup)
    {
        if (popup == null)
        {
            Debug.LogWarning($"[{_name}] ClosePopupUI: 팝업이 null입니다.");
            return;
        }

        // 최상위 팝업이 아니면 스택에서 찾아서 제거
        if (_popupStack.Count > 0 && _popupStack.Peek() != popup)
        {
            Debug.LogWarning($"[{_name}] ClosePopupUI: 최상위 팝업이 아닙니다.");
            return;
        }

        // 스택에서 제거
        _popupStack.Pop();

        // 게임 오브젝트 파괴
        Object.Destroy(popup.gameObject);
    }

    /// <summary>
    /// 최상위 팝업 UI 닫기
    /// </summary>
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
        {
            Debug.LogWarning($"[{_name}] ClosePopupUI: 열려있는 팝업이 없습니다.");
            return;
        }

        // 최상위 팝업 가져오기
        UI_Popup popup = _popupStack.Peek();

        // 팝업 닫기
        ClosePopupUI(popup);
    }

    /// <summary>
    /// 모든 팝업 UI 닫기
    /// </summary>
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            UI_Popup popup = _popupStack.Pop();
            Object.Destroy(popup.gameObject);
        }
    }

    /// <summary>
    /// 월드 스페이스 UI 생성
    /// </summary>
    /// <typeparam name="T">월드 스페이스 UI 타입</typeparam>
    /// <param name="name">리소스 이름</param>
    /// <param name="parent">부모 Transform</param>
    /// <returns>생성된 월드 스페이스 UI</returns>
    public T CreateWorldSpaceUI<T>(string name = null, Transform parent = null) where T : UI_Base
    {
        // 리소스 이름이 없으면 타입 이름 사용
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        // 월드 스페이스 UI 생성
        GameObject go = _resourceManager.Instantiate($"UI/WorldSpace/{name}");
        if (go == null)
        {
            Debug.LogError($"[{_name}] CreateWorldSpaceUI: 월드 스페이스 UI를 찾을 수 없습니다. 경로: UI/WorldSpace/{name}");
            return null;
        }

        // 부모 설정
        if (parent == null)
        {
            go.transform.SetParent(_worldSpaceUIRoot.transform);
        }
        else
        {
            go.transform.SetParent(parent);
        }

        // 위치 및 스케일 초기화
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        // 컴포넌트 가져오기
        T worldSpaceUI = Utils.GetOrAddComponent<T>(go);

        return worldSpaceUI;
    }
} 
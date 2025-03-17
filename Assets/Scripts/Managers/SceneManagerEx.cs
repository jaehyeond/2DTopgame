using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

/// <summary>
/// 씬 전환 및 관리를 담당하는 매니저
/// </summary>
public class SceneManagerEx : BaseManager
{
    /// <summary>
    /// 현재 씬 타입
    /// </summary>
    private Define.EScene _currentScene = Define.EScene.Unknown;
    public Define.EScene CurrentScene => _currentScene;

    /// <summary>
    /// 씬 전환 중 여부
    /// </summary>
    private bool _isLoading = false;
    public bool IsLoading => _isLoading;

    /// <summary>
    /// 씬 전환 완료 이벤트
    /// </summary>
    public event Action<Define.EScene> OnSceneLoaded;

    /// <summary>
    /// 씬 전환 시작 이벤트
    /// </summary>
    public event Action<Define.EScene> OnSceneUnloaded;

    /// <summary>
    /// 로딩 진행도
    /// </summary>
    private float _loadingProgress = 0f;
    public float LoadingProgress => _loadingProgress;

    /// <summary>
    /// 의존성 주입
    /// </summary>
    [Inject]
    public SceneManagerEx()
    {
    }

    /// <summary>
    /// 매니저 초기화 구현
    /// </summary>
    protected override IEnumerator OnInitialize()
    {
        Debug.Log($"<color=yellow>[{_name}] 초기화 중...</color>");

        // 현재 씬 타입 설정
        _currentScene = GetSceneType(SceneManager.GetActiveScene().name);

        // 씬 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoadedEvent;
        SceneManager.sceneUnloaded += OnSceneUnloadedEvent;

        yield return null;
    }

    /// <summary>
    /// 매니저 종료 구현
    /// </summary>
    protected override IEnumerator OnTerminate()
    {
        Debug.Log($"<color=yellow>[{_name}] 종료 중...</color>");

        // 씬 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoadedEvent;
        SceneManager.sceneUnloaded -= OnSceneUnloadedEvent;

        yield return null;
    }

    /// <summary>
    /// 씬 로드 이벤트 핸들러
    /// </summary>
    private void OnSceneLoadedEvent(Scene scene, LoadSceneMode mode)
    {
        _currentScene = GetSceneType(scene.name);
        _isLoading = false;
        Debug.Log($"<color=green>[{_name}] 씬 로드 완료: {_currentScene}</color>");
    }

    /// <summary>
    /// 씬 언로드 이벤트 핸들러
    /// </summary>
    private void OnSceneUnloadedEvent(Scene scene)
    {
        Debug.Log($"<color=yellow>[{_name}] 씬 언로드 완료: {GetSceneType(scene.name)}</color>");
    }

    /// <summary>
    /// 씬 이름으로 씬 타입 가져오기
    /// </summary>
    /// <param name="sceneName">씬 이름</param>
    /// <returns>씬 타입</returns>
    private Define.EScene GetSceneType(string sceneName)
    {
        if (Enum.TryParse(sceneName, out Define.EScene sceneType))
        {
            return sceneType;
        }

        return Define.EScene.Unknown;
    }

    /// <summary>
    /// 씬 타입으로 씬 이름 가져오기
    /// </summary>
    /// <param name="sceneType">씬 타입</param>
    /// <returns>씬 이름</returns>
    private string GetSceneName(Define.EScene sceneType)
    {
        return sceneType.ToString();
    }

    /// <summary>
    /// 씬 로드
    /// </summary>
    /// <param name="sceneType">로드할 씬 타입</param>
    /// <param name="loadingScene">로딩 씬 사용 여부</param>
    public void LoadScene(Define.EScene sceneType, bool loadingScene = false)
    {
        if (_isLoading)
        {
            Debug.LogWarning($"[{_name}] 이미 씬 로딩 중입니다.");
            return;
        }

        if (_currentScene == sceneType)
        {
            Debug.LogWarning($"[{_name}] 이미 같은 씬에 있습니다: {sceneType}");
            return;
        }

        _isLoading = true;

        if (loadingScene)
        {
            // 로딩 씬을 통한 씬 전환
            CoroutineRunner.Instance.RunCoroutine(LoadSceneWithLoading(sceneType));
        }
        else
        {
            // 직접 씬 전환
            CoroutineRunner.Instance.RunCoroutine(LoadSceneAsync(sceneType));
        }
    }

    /// <summary>
    /// 비동기 씬 로드
    /// </summary>
    /// <param name="sceneType">로드할 씬 타입</param>
    private IEnumerator LoadSceneAsync(Define.EScene sceneType)
    {
        Debug.Log($"<color=yellow>[{_name}] 씬 로드 시작: {sceneType}</color>");

        // 이전 씬 언로드 이벤트 발생
        OnSceneUnloaded?.Invoke(_currentScene);

        // 씬 로드
        string sceneName = GetSceneName(sceneType);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 로딩 진행도 업데이트
        while (!operation.isDone)
        {
            _loadingProgress = operation.progress;
            yield return null;
        }

        _loadingProgress = 1f;

        // 씬 로드 완료 이벤트 발생
        OnSceneLoaded?.Invoke(sceneType);
    }

    /// <summary>
    /// 로딩 씬을 통한 씬 로드
    /// </summary>
    /// <param name="sceneType">로드할 씬 타입</param>
    private IEnumerator LoadSceneWithLoading(Define.EScene sceneType)
    {
        Debug.Log($"<color=yellow>[{_name}] 로딩 씬을 통한 씬 로드 시작: {sceneType}</color>");

        // 이전 씬 언로드 이벤트 발생
        OnSceneUnloaded?.Invoke(_currentScene);

        // 로딩 씬 로드
        yield return LoadSceneAsync(Define.EScene.Loading);

        // 실제 씬 로드
        yield return LoadSceneAsync(sceneType);
    }

    /// <summary>
    /// 씬 재로드
    /// </summary>
    /// <param name="loadingScene">로딩 씬 사용 여부</param>
    public void ReloadScene(bool loadingScene = false)
    {
        LoadScene(_currentScene, loadingScene);
    }
} 
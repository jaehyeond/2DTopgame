using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 게임의 중앙 제어 시스템으로, 모든 매니저와 시스템을 관리하는 클래스
/// </summary>
public class ModularApplicationController : LifetimeScope
{
    /// <summary>
    /// 애플리케이션 상태
    /// </summary>
    public enum ApplicationState
    {
        None,           // 초기 상태
        Initializing,   // 초기화 중
        Running,        // 실행 중
        Paused,         // 일시 정지
        Terminating     // 종료 중
    }

    /// <summary>
    /// 현재 애플리케이션 상태
    /// </summary>
    private ApplicationState _currentState = ApplicationState.None;

    /// <summary>
    /// 애플리케이션 상태 변경 이벤트
    /// </summary>
    public event Action<ApplicationState> OnStateChanged;

    /// <summary>
    /// 싱글톤 인스턴스
    /// </summary>
    private static ModularApplicationController _instance;
    public static ModularApplicationController Instance => _instance;

    /// <summary>
    /// VContainer 컨테이너
    /// </summary>
    public new IObjectResolver Container { get; private set; }

    /// <summary>
    /// VContainer 의존성 주입 설정
    /// </summary>
    protected override void Configure(IContainerBuilder builder)
    {
        // 싱글톤으로 등록할 매니저 클래스들
        // 주의: 아직 구현되지 않은 매니저는 주석 처리
        
        // 코루틴 실행을 위한 MonoBehaviour 래퍼 등록
        builder.RegisterComponentInHierarchy<CoroutineRunner>().AsSelf().AsImplementedInterfaces();
        
        // 매니저 등록
        builder.Register<SceneManagerEx>(Lifetime.Singleton);
        builder.Register<UIManager>(Lifetime.Singleton);
        
        // 나중에 구현할 매니저들
        // builder.Register<GameManager>(Lifetime.Singleton);
        // builder.Register<ResourceManager>(Lifetime.Singleton);
        // builder.Register<SoundManager>(Lifetime.Singleton);
        // builder.Register<DataManager>(Lifetime.Singleton);
        
        // 시스템 등록
        // builder.Register<InputSystem>(Lifetime.Singleton);
        // builder.Register<TowerSystem>(Lifetime.Singleton);
        // builder.Register<CombatSystem>(Lifetime.Singleton);
        // builder.Register<ProgressionSystem>(Lifetime.Singleton);
        
        // 서비스 등록
        // builder.Register<LoggingService>(Lifetime.Singleton);
        // builder.Register<AnalyticsService>(Lifetime.Singleton);
        
        base.Configure(builder);
    }

    /// <summary>
    /// 애플리케이션 초기화
    /// </summary>
    public void Initialize()
    {
        if (_currentState != ApplicationState.None)
            return;

        ChangeState(ApplicationState.Initializing);

        // 초기화 로직 실행
        StartCoroutine(InitializeCoroutine());
    }

    /// <summary>
    /// 초기화 코루틴
    /// </summary>
    private IEnumerator InitializeCoroutine()
    {
        Debug.Log("<color=green>[ModularApplicationController] 애플리케이션 초기화 시작</color>");

        // 각 매니저 초기화
        yield return InitializeManagers();

        // 각 시스템 초기화
        yield return InitializeSystems();

        // 초기화 완료
        ChangeState(ApplicationState.Running);
        Debug.Log("<color=green>[ModularApplicationController] 애플리케이션 초기화 완료</color>");
    }

    /// <summary>
    /// 매니저 초기화
    /// </summary>
    private IEnumerator InitializeManagers()
    {
        Debug.Log("<color=yellow>[ModularApplicationController] 매니저 초기화 시작</color>");

        // 각 매니저 초기화 로직
        // 실제 구현 시 각 매니저의 초기화 메서드 호출

        yield return null;

        Debug.Log("<color=yellow>[ModularApplicationController] 매니저 초기화 완료</color>");
    }

    /// <summary>
    /// 시스템 초기화
    /// </summary>
    private IEnumerator InitializeSystems()
    {
        Debug.Log("<color=yellow>[ModularApplicationController] 시스템 초기화 시작</color>");

        // 각 시스템 초기화 로직
        // 실제 구현 시 각 시스템의 초기화 메서드 호출

        yield return null;

        Debug.Log("<color=yellow>[ModularApplicationController] 시스템 초기화 완료</color>");
    }

    /// <summary>
    /// 애플리케이션 상태 변경
    /// </summary>
    private void ChangeState(ApplicationState newState)
    {
        if (_currentState == newState)
            return;

        Debug.Log($"<color=blue>[ModularApplicationController] 상태 변경: {_currentState} -> {newState}</color>");
        _currentState = newState;
        OnStateChanged?.Invoke(newState);
    }

    /// <summary>
    /// 애플리케이션 일시 정지
    /// </summary>
    public void Pause()
    {
        if (_currentState != ApplicationState.Running)
            return;

        ChangeState(ApplicationState.Paused);
    }

    /// <summary>
    /// 애플리케이션 재개
    /// </summary>
    public void Resume()
    {
        if (_currentState != ApplicationState.Paused)
            return;

        ChangeState(ApplicationState.Running);
    }

    /// <summary>
    /// 애플리케이션 종료
    /// </summary>
    public void Terminate()
    {
        if (_currentState == ApplicationState.Terminating)
            return;

        ChangeState(ApplicationState.Terminating);
        StartCoroutine(TerminateCoroutine());
    }

    /// <summary>
    /// 종료 코루틴
    /// </summary>
    private IEnumerator TerminateCoroutine()
    {
        Debug.Log("<color=red>[ModularApplicationController] 애플리케이션 종료 시작</color>");

        // 각 시스템 종료
        yield return TerminateSystems();

        // 각 매니저 종료
        yield return TerminateManagers();

        Debug.Log("<color=red>[ModularApplicationController] 애플리케이션 종료 완료</color>");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// 시스템 종료
    /// </summary>
    private IEnumerator TerminateSystems()
    {
        Debug.Log("<color=yellow>[ModularApplicationController] 시스템 종료 시작</color>");

        // 각 시스템 종료 로직
        // 실제 구현 시 각 시스템의 종료 메서드 호출

        yield return null;

        Debug.Log("<color=yellow>[ModularApplicationController] 시스템 종료 완료</color>");
    }

    /// <summary>
    /// 매니저 종료
    /// </summary>
    private IEnumerator TerminateManagers()
    {
        Debug.Log("<color=yellow>[ModularApplicationController] 매니저 종료 시작</color>");

        // 각 매니저 종료 로직
        // 실제 구현 시 각 매니저의 종료 메서드 호출

        yield return null;

        Debug.Log("<color=yellow>[ModularApplicationController] 매니저 종료 완료</color>");
    }

    /// <summary>
    /// 초기화
    /// </summary>
    protected override void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        base.Awake();
    }

    /// <summary>
    /// 애플리케이션 시작 시 자동 초기화
    /// </summary>
    private void Start()
    {
        // 게임 초기화
        Initialize();
        
        // 초기화 완료 후 MainMenu 씬으로 전환
        StartCoroutine(LoadMainMenuAfterInitialization());
    }

    private IEnumerator LoadMainMenuAfterInitialization()
    {
        // 로딩 UI 요소 찾기
        Slider progressBar = GameObject.Find("ProgressBar")?.GetComponent<Slider>();
        TextMeshProUGUI loadingText = GameObject.Find("LoadingText")?.GetComponent<TextMeshProUGUI>();
        
        // 초기화 진행 상태 표시
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * 0.5f; // 시뮬레이션된 로딩 진행
            if (progressBar != null)
                progressBar.value = progress;
            
            // 로딩 텍스트 업데이트
            if (loadingText != null)
            {
                if (progress < 0.3f)
                    loadingText.text = "게임 초기화 중...";
                else if (progress < 0.6f)
                    loadingText.text = "리소스 로드 중...";
                else
                    loadingText.text = "준비 완료!";
            }
            
            yield return null;
        }
        
        // 잠시 대기 후 메인 메뉴로 전환
        yield return new WaitForSeconds(1f);
        
        // SceneManagerEx를 통해 MainMenu 씬으로 전환
        var sceneManager = Container.Resolve<SceneManagerEx>();
        sceneManager.LoadScene(Define.EScene.MainMenu, true);
    }

    /// <summary>
    /// 애플리케이션 종료 시 정리
    /// </summary>
    protected override void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
        
        base.OnDestroy(); // 부모 클래스의 OnDestroy 호출
    }
} 
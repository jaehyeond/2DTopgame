using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

/// <summary>
/// 게임 리소스를 로드하고 관리하는 매니저
/// </summary>
public class ResourceManager : BaseManager
{
    /// <summary>
    /// 리소스 캐시
    /// </summary>
    private Dictionary<string, Object> _resources = new Dictionary<string, Object>();

    /// <summary>
    /// 프리팹 캐시
    /// </summary>
    private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

    /// <summary>
    /// 스프라이트 캐시
    /// </summary>
    private Dictionary<string, Sprite> _spriteCache = new Dictionary<string, Sprite>();

    /// <summary>
    /// 오디오 클립 캐시
    /// </summary>
    private Dictionary<string, AudioClip> _audioCache = new Dictionary<string, AudioClip>();

    /// <summary>
    /// 머티리얼 캐시
    /// </summary>
    private Dictionary<string, Material> _materialCache = new Dictionary<string, Material>();

    /// <summary>
    /// 의존성 주입
    /// </summary>
    [Inject]
    public ResourceManager()
    {
    }

    /// <summary>
    /// 매니저 초기화 구현
    /// </summary>
    protected override IEnumerator OnInitialize()
    {
        Debug.Log($"<color=yellow>[{_name}] 초기화 중...</color>");

        // 기본 리소스 로드
        yield return PreloadResources();

        Debug.Log($"<color=green>[{_name}] 초기화 완료</color>");
    }

    /// <summary>
    /// 매니저 종료 구현
    /// </summary>
    protected override IEnumerator OnTerminate()
    {
        Debug.Log($"<color=yellow>[{_name}] 종료 중...</color>");

        // 캐시 정리
        ClearCache();

        yield return null;
    }

    /// <summary>
    /// 기본 리소스 미리 로드
    /// </summary>
    private IEnumerator PreloadResources()
    {
        Debug.Log($"<color=yellow>[{_name}] 기본 리소스 로드 중...</color>");

        // 자주 사용되는 UI 프리팹 미리 로드
        LoadPrefab("UI/PopupCommon");
        LoadPrefab("UI/PopupConfirm");

        // 자주 사용되는 게임 오브젝트 미리 로드
        // LoadPrefab("Player/PlayerCharacter");

        yield return null;
    }

    /// <summary>
    /// 리소스 캐시 정리
    /// </summary>
    public void ClearCache()
    {
        _resources.Clear();
        _prefabCache.Clear();
        _spriteCache.Clear();
        _audioCache.Clear();
        _materialCache.Clear();

        // 가비지 컬렉션 요청
        Resources.UnloadUnusedAssets();
        GC.Collect();

        Debug.Log($"<color=green>[{_name}] 캐시 정리 완료</color>");
    }

    /// <summary>
    /// 리소스 로드
    /// </summary>
    public T Load<T>(string path) where T : Object
    {
        // 이미 캐시된 리소스인지 확인
        if (_resources.TryGetValue(path, out Object resource))
        {
            return resource as T;
        }

        // 리소스 로드
        T loadedResource = Resources.Load<T>(path);
        if (loadedResource == null)
        {
            Debug.LogError($"[{_name}] 리소스를 찾을 수 없습니다: {path}");
            return null;
        }

        // 캐시에 추가
        _resources.Add(path, loadedResource);

        return loadedResource;
    }

    /// <summary>
    /// 프리팹 로드 및 인스턴스화
    /// </summary>
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = LoadPrefab(path);
        if (prefab == null)
        {
            Debug.LogError($"[{_name}] 프리팹을 찾을 수 없습니다: {path}");
            return null;
        }

        // 프리팹 인스턴스화
        GameObject instance = Object.Instantiate(prefab, parent);
        instance.name = prefab.name;

        return instance;
    }

    /// <summary>
    /// 프리팹 로드
    /// </summary>
    public GameObject LoadPrefab(string path)
    {
        // 이미 캐시된 프리팹인지 확인
        if (_prefabCache.TryGetValue(path, out GameObject prefab))
        {
            return prefab;
        }

        // 프리팹 로드
        prefab = Load<GameObject>(path);
        if (prefab != null)
        {
            _prefabCache.Add(path, prefab);
        }

        return prefab;
    }

    /// <summary>
    /// 스프라이트 로드
    /// </summary>
    public Sprite LoadSprite(string path)
    {
        // 이미 캐시된 스프라이트인지 확인
        if (_spriteCache.TryGetValue(path, out Sprite sprite))
        {
            return sprite;
        }

        // 스프라이트 로드
        sprite = Load<Sprite>(path);
        if (sprite != null)
        {
            _spriteCache.Add(path, sprite);
        }

        return sprite;
    }

    /// <summary>
    /// 오디오 클립 로드
    /// </summary>
    public AudioClip LoadAudioClip(string path)
    {
        // 이미 캐시된 오디오 클립인지 확인
        if (_audioCache.TryGetValue(path, out AudioClip clip))
        {
            return clip;
        }

        // 오디오 클립 로드
        clip = Load<AudioClip>(path);
        if (clip != null)
        {
            _audioCache.Add(path, clip);
        }

        return clip;
    }

    /// <summary>
    /// 게임 오브젝트 파괴
    /// </summary>
    public void Destroy(GameObject obj)
    {
        if (obj == null)
            return;
            
        Object.Destroy(obj);
    }

    /// <summary>
    /// 머티리얼 로드
    /// </summary>
    /// <param name="path">리소스 경로</param>
    /// <param name="useCache">캐시 사용 여부</param>
    /// <returns>로드된 머티리얼</returns>
    public Material LoadMaterial(string path, bool useCache = true)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning($"[{_name}] LoadMaterial: 경로가 비어있습니다.");
            return null;
        }

        // 캐시 확인
        if (useCache && _materialCache.TryGetValue(path, out Material cachedMaterial))
        {
            return cachedMaterial;
        }

        // 리소스 로드
        Material material = Resources.Load<Material>(path);
        if (material == null)
        {
            Debug.LogWarning($"[{_name}] LoadMaterial: 머티리얼을 찾을 수 없습니다. 경로: {path}");
            return null;
        }

        // 캐시 저장
        if (useCache)
        {
            _materialCache[path] = material;
        }

        return material;
    }
} 
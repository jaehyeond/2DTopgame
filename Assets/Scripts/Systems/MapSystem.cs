using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

/// <summary>
/// 맵 생성 및 관리 시스템
/// </summary>
public class MapSystem : BaseSystem
{
    /// <summary>
    /// 맵 타입
    /// </summary>
    public enum MapType
    {
        None,
        Dungeon,
        Tower,
        Boss
    }
    
    /// <summary>
    /// 현재 맵 타입
    /// </summary>
    private MapType _currentMapType = MapType.None;
    public MapType CurrentMapType => _currentMapType;
    
    /// <summary>
    /// 리소스 매니저
    /// </summary>
    private ResourceManager _resourceManager;
    
    /// <summary>
    /// 타일맵 참조
    /// </summary>
    private Tilemap _groundTilemap;
    private Tilemap _wallTilemap;
    private Tilemap _decorationTilemap;
    
    /// <summary>
    /// 맵 크기
    /// </summary>
    private int _mapWidth = 50;
    private int _mapHeight = 50;
    
    /// <summary>
    /// 맵 데이터
    /// </summary>
    private int[,] _mapData;
    
    /// <summary>
    /// 의존성 주입
    /// </summary>
    [Inject]
    public MapSystem(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }
    
    /// <summary>
    /// 시스템 초기화 구현
    /// </summary>
    protected override IEnumerator OnInitialize()
    {
        Debug.Log($"<color=yellow>[{_name}] 초기화 중...</color>");
        
        // 타일맵 참조 가져오기
        yield return FindTilemaps();
        
        Debug.Log($"<color=green>[{_name}] 초기화 완료</color>");
    }
    
    /// <summary>
    /// 시스템 종료 구현
    /// </summary>
    protected override IEnumerator OnTerminate()
    {
        Debug.Log($"<color=yellow>[{_name}] 종료 중...</color>");
        
        // 맵 데이터 정리
        ClearMap();
        
        yield return null;
    }
    
    /// <summary>
    /// 타일맵 참조 가져오기
    /// </summary>
    private IEnumerator FindTilemaps()
    {
        // 타일맵 게임 오브젝트 찾기
        GameObject tilemapObject = GameObject.Find("Grid");
        if (tilemapObject == null)
        {
            Debug.LogWarning($"[{_name}] 타일맵 그리드를 찾을 수 없습니다.");
            yield break;
        }
        
        // 타일맵 컴포넌트 가져오기
        _groundTilemap = tilemapObject.transform.Find("Ground")?.GetComponent<Tilemap>();
        _wallTilemap = tilemapObject.transform.Find("Wall")?.GetComponent<Tilemap>();
        _decorationTilemap = tilemapObject.transform.Find("Decoration")?.GetComponent<Tilemap>();
        
        if (_groundTilemap == null || _wallTilemap == null)
        {
            Debug.LogWarning($"[{_name}] 필요한 타일맵을 찾을 수 없습니다.");
        }
        
        yield return null;
    }
    
    /// <summary>
    /// 맵 생성
    /// </summary>
    public void GenerateMap(MapType mapType, int width = 50, int height = 50)
    {
        // 이전 맵 정리
        ClearMap();
        
        // 맵 설정
        _currentMapType = mapType;
        _mapWidth = width;
        _mapHeight = height;
        _mapData = new int[_mapWidth, _mapHeight];
        
        // 맵 타입에 따라 생성 방식 선택
        switch (mapType)
        {
            case MapType.Dungeon:
                GenerateDungeonMap();
                break;
            case MapType.Tower:
                GenerateTowerMap();
                break;
            case MapType.Boss:
                GenerateBossMap();
                break;
            default:
                Debug.LogWarning($"[{_name}] 알 수 없는 맵 타입: {mapType}");
                break;
        }
    }
    
    /// <summary>
    /// 던전 맵 생성
    /// </summary>
    private void GenerateDungeonMap()
    {
        Debug.Log($"<color=yellow>[{_name}] 던전 맵 생성 중...</color>");
        
        // 던전 맵 생성 알고리즘 구현
        // 여기서는 간단한 랜덤 맵 생성
        System.Random random = new System.Random();
        
        // 맵 데이터 초기화 (모두 벽으로 설정)
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                _mapData[x, y] = 1; // 1 = 벽
            }
        }
        
        // 중앙에서 시작하는 방 생성
        int centerX = _mapWidth / 2;
        int centerY = _mapHeight / 2;
        int roomWidth = random.Next(5, 10);
        int roomHeight = random.Next(5, 10);
        
        // 방 영역을 바닥으로 설정
        for (int x = centerX - roomWidth / 2; x < centerX + roomWidth / 2; x++)
        {
            for (int y = centerY - roomHeight / 2; y < centerY + roomHeight / 2; y++)
            {
                if (x >= 0 && x < _mapWidth && y >= 0 && y < _mapHeight)
                {
                    _mapData[x, y] = 0; // 0 = 바닥
                }
            }
        }
        
        // 맵 데이터를 타일맵에 적용
        ApplyMapDataToTilemap();
        
        Debug.Log($"<color=green>[{_name}] 던전 맵 생성 완료</color>");
    }
    
    /// <summary>
    /// 타워 맵 생성
    /// </summary>
    private void GenerateTowerMap()
    {
        Debug.Log($"<color=yellow>[{_name}] 타워 맵 생성 중...</color>");
        
        // 타워 맵 생성 알고리즘 구현
        // 여기서는 간단한 타워 층 생성
        
        // 맵 데이터 초기화 (모두 벽으로 설정)
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                _mapData[x, y] = 1; // 1 = 벽
            }
        }
        
        // 타워 내부 영역을 바닥으로 설정
        int margin = 5;
        for (int x = margin; x < _mapWidth - margin; x++)
        {
            for (int y = margin; y < _mapHeight - margin; y++)
            {
                _mapData[x, y] = 0; // 0 = 바닥
            }
        }
        
        // 맵 데이터를 타일맵에 적용
        ApplyMapDataToTilemap();
        
        Debug.Log($"<color=green>[{_name}] 타워 맵 생성 완료</color>");
    }
    
    /// <summary>
    /// 보스 맵 생성
    /// </summary>
    private void GenerateBossMap()
    {
        Debug.Log($"<color=yellow>[{_name}] 보스 맵 생성 중...</color>");
        
        // 보스 맵 생성 알고리즘 구현
        // 여기서는 간단한 원형 방 생성
        
        // 맵 데이터 초기화 (모두 벽으로 설정)
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                _mapData[x, y] = 1; // 1 = 벽
            }
        }
        
        // 중앙에 원형 방 생성
        int centerX = _mapWidth / 2;
        int centerY = _mapHeight / 2;
        int radius = 15;
        
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                // 중앙으로부터의 거리 계산
                float distance = Mathf.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                
                // 반경 내부는 바닥으로 설정
                if (distance < radius)
                {
                    _mapData[x, y] = 0; // 0 = 바닥
                }
            }
        }
        
        // 맵 데이터를 타일맵에 적용
        ApplyMapDataToTilemap();
        
        Debug.Log($"<color=green>[{_name}] 보스 맵 생성 완료</color>");
    }
    
    /// <summary>
    /// 맵 데이터를 타일맵에 적용
    /// </summary>
    private void ApplyMapDataToTilemap()
    {
        if (_groundTilemap == null || _wallTilemap == null)
        {
            Debug.LogWarning($"[{_name}] 타일맵이 설정되지 않았습니다.");
            return;
        }
        
        // 타일 에셋 로드
        TileBase groundTile = _resourceManager.Load<TileBase>("Tiles/GroundTile");
        TileBase wallTile = _resourceManager.Load<TileBase>("Tiles/WallTile");
        
        if (groundTile == null || wallTile == null)
        {
            Debug.LogWarning($"[{_name}] 타일 에셋을 찾을 수 없습니다.");
            return;
        }
        
        // 타일맵 초기화
        _groundTilemap.ClearAllTiles();
        _wallTilemap.ClearAllTiles();
        
        // 맵 데이터에 따라 타일 설정
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                Vector3Int tilePos = new Vector3Int(x - _mapWidth / 2, y - _mapHeight / 2, 0);
                
                if (_mapData[x, y] == 0) // 바닥
                {
                    _groundTilemap.SetTile(tilePos, groundTile);
                }
                else if (_mapData[x, y] == 1) // 벽
                {
                    _wallTilemap.SetTile(tilePos, wallTile);
                }
            }
        }
    }
    
    /// <summary>
    /// 맵 정리
    /// </summary>
    public void ClearMap()
    {
        // 맵 데이터 초기화
        _mapData = null;
        _currentMapType = MapType.None;
        
        // 타일맵 초기화
        if (_groundTilemap != null)
            _groundTilemap.ClearAllTiles();
            
        if (_wallTilemap != null)
            _wallTilemap.ClearAllTiles();
            
        if (_decorationTilemap != null)
            _decorationTilemap.ClearAllTiles();
    }
} 
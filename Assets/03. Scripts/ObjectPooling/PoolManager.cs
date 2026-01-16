using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [SerializeField] private Transform uiPoolRoot;

    [System.Serializable]
    public class PoolConfig
    {
        public string id;              // 풀 ID
        public GameObject prefab;      // 풀 프리팹
        public int preloadCount = 10;  // 미리 생성할 개수
        public bool isUI;              // UI 풀 여부
    }

    [Header("Pool Configs")]
    [SerializeField] private List<PoolConfig> poolConfigs = new();

    private Dictionary<string, ObjectPool<Component>> poolDict
        = new Dictionary<string, ObjectPool<Component>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePools();
    }

    // 풀 초기화
    private void InitializePools()
    {
        foreach (var config in poolConfigs)
        {
            if (string.IsNullOrEmpty(config.id) || config.prefab == null)
                continue;

            var comp = config.prefab.GetComponent<Component>();
            if (comp == null)
            {
                Debug.LogError($"[{config.id}] prefab has no Component");
                continue;
            }

            // UI 여부에 따라 부모 결정
            Transform parent = config.isUI && uiPoolRoot != null
                ? uiPoolRoot
                : transform;

            var pool = new ObjectPool<Component>(
                comp,
                config.preloadCount,
                config.id,
                parent
            );

            poolDict.Add(config.id, pool);
        }
    }

    // 풀에서 꺼내기
    public GameObject Spawn(string id)
    {
        if (!poolDict.TryGetValue(id, out var pool))
            return null;

        Component comp = pool.Get();
        return comp.gameObject;
    }

    // 풀로 반환
    public void Despawn(string id, GameObject obj)
    {
        if (!poolDict.TryGetValue(id, out var pool))
        {
            Destroy(obj);
            return;
        }

        var pooled = obj.GetComponent<PooledObject>();
        if (pooled != null && pooled.releaseComponent != null)
        {
            pool.Release(pooled.releaseComponent);
            return;
        }

        Destroy(obj);
    }

    // 오브젝트 기준 반환
    public void Despawn(GameObject obj)
    {
        if (obj == null) return;

        var pooled = obj.GetComponent<PooledObject>();
        if (pooled == null || string.IsNullOrEmpty(pooled.poolId))
        {
            Destroy(obj);
            return;
        }

        Despawn(pooled.poolId, obj);
    }

    public void ResetPool(string id)
    {
        if (!poolDict.TryGetValue(id, out var pool))
            return;

        pool.ResetPool();
    }

}

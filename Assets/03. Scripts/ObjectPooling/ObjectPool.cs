using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly Queue<T> pool = new Queue<T>();
    private readonly HashSet<T> activeSet = new HashSet<T>();

    private readonly T prefab;
    private readonly Transform parent;
    private readonly string poolId;

    public int Count => pool.Count;

    public ObjectPool(T prefab, int preloadCount, string poolId, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.poolId = poolId;

        for (int i = 0; i < preloadCount; i++)
        {
            T obj = CreateNew();
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    private T CreateNew()
    {
        T obj = Object.Instantiate(prefab, parent);
        obj.gameObject.name = poolId;

        var pooled = obj.GetComponent<PooledObject>();
        if (pooled == null)
            pooled = obj.gameObject.AddComponent<PooledObject>();

        pooled.poolId = poolId;
        pooled.releaseComponent = obj;

        return obj;
    }

    public T Get()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : CreateNew();

        obj.gameObject.name = poolId;
        obj.gameObject.SetActive(true);

        activeSet.Add(obj);
        return obj;
    }

    public void Release(T obj)
    {
        if (obj == null) return;

        if (!activeSet.Remove(obj))
            return;

        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    public void ResetPool()
    {
        foreach (var obj in activeSet)
        {
            if (obj == null) continue;

            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

        activeSet.Clear();
    }
}

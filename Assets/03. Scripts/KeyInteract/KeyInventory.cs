using UnityEngine;
using System;
using System.Collections.Generic;

public class KeyInventory : MonoBehaviour
{
    public static KeyInventory Instance;

    public event Action<int> OnKeyCountChanged;

    private HashSet<string> keys = new();

    public int KeyCount => keys.Count;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public bool AddKey(string keyId)
    {
        bool added = keys.Add(keyId);
        if (added)
            OnKeyCountChanged?.Invoke(keys.Count);
        return added;
    }

    public bool RemoveKey(string keyId)
    {
        bool removed = keys.Remove(keyId);
        if (removed)
            OnKeyCountChanged?.Invoke(keys.Count);
        return removed;
    }

    public bool HasKey(string keyId)
    {
        return keys.Contains(keyId);
    }

    public bool HasAnyKey()
    {
        return keys.Count > 0;
    }

    // =========================
    // 저장/로드 전용 API
    // =========================

    public List<string> GetAllKeys()
    {
        return new List<string>(keys);
    }

    public void RestoreKeys(List<string> savedKeys)
    {
        keys.Clear();

        if (savedKeys == null)
        {
            OnKeyCountChanged?.Invoke(0);
            return;
        }

        foreach (var key in savedKeys)
            keys.Add(key);

        OnKeyCountChanged?.Invoke(keys.Count);
    }
}

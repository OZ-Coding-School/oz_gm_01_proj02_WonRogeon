using UnityEngine;

/// <summary>
/// 플레이어 싱글톤 (Zone 이동용)
/// </summary>
public class Player : MonoBehaviour
{
    public static Player Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}

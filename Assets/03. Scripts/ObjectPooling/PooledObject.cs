using UnityEngine;

// 풀 반환 전용 컴포넌트
public class PooledObject : MonoBehaviour
{
    [HideInInspector] public string poolId;
    [HideInInspector] public Component releaseComponent;

    // 외부에서 호출용
    public void ReturnToPool()
    {
        if (PoolManager.Instance == null)
        {
            Destroy(gameObject);
            return;
        }

        PoolManager.Instance.Despawn(gameObject);
    }
}

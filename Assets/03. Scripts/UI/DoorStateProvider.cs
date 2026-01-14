using UnityEngine;
using System;
/// <summary>
/// 문 상태 제공 전용 컴포넌트
/// (잠김 / 열림 여부만 알려줌)
/// </summary>
public class DoorStateProvider : MonoBehaviour
{
    [SerializeField] private bool isLocked = true;

    public bool IsLocked => isLocked;

    // 상태 변경 이벤트
    public event Action OnDoorUnlocked;

    public void Unlock()
    {
        if (!isLocked)
            return;

        isLocked = false;
        OnDoorUnlocked?.Invoke();
    }
}
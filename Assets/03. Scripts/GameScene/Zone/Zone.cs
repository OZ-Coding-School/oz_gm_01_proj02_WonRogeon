using UnityEngine;

/// <summary>
/// GameScene 내부의 하나의 구역(Zone)
/// 맵 조각이 아닌 하나의 규칙 묶음이자 연출 단위
/// 씬 전환 없이도 장면을 바꾼 것처럼 느끼게 만듦
/// </summary>
public class Zone : MonoBehaviour
{
    public string zoneName;

    [Header("Camera Settings")]
    public bool followCamera = false;
    public Transform cameraFixedPoint;

    public virtual void OnEnter()
    {
        Debug.Log($"[Zone Enter] {zoneName}");

        if (followCamera)
        {
            CameraController.Instance.SetFollow(Player.Instance.transform);
        }
        else if (cameraFixedPoint != null)
        {
            CameraController.Instance.SetFixed(cameraFixedPoint.position);
        }
    }

    public virtual void OnExit()
    {
        Debug.Log($"[Zone Exit] {zoneName}");
    }

    // virtual 가상메서드로 만든 이유는 나중에 미래 확장용으로 
    // 이벤트zone이나 컷신zone 등을 만들 때를 대비한 것
}

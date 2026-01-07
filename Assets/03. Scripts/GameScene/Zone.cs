using UnityEngine;

/// <summary>
/// GameScene 내부의 하나의 구역(Zone)
/// </summary>
public class Zone : MonoBehaviour
{
    public string zoneName;

    [Header("Player Spawn Point")]
    public Transform spawnPoint;

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
}

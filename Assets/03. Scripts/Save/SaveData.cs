using System;

[Serializable]
public class SaveData
{
    public string floorAndRoom;   // 예: "1F Aya Room"
    public float playTimeSeconds; // 누적 플레이 시간
    public string savedAt;        // 저장 시각 (yyyy-MM-dd HH:mm)
}

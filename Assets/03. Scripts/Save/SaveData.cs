using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    // UI 표시용
    public string floorAndRoom;

    // 로드용 (정확한 식별자)
    public int floor;
    public string zoneName;

    public float playTimeSeconds;
    public string savedAt;

    // 열쇠 저장
    public List<string> ownedKeys;
}

using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string SaveDir => Path.Combine(Application.persistentDataPath, "saves");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save(int slotIndex)
    {
        if (!Directory.Exists(SaveDir))
            Directory.CreateDirectory(SaveDir);

        Zone current = ZoneManager.Instance.CurrentZone;

        SaveData data = new SaveData
        {
            floorAndRoom = current.DisplayName,
            floor = current.floor,
            zoneName = current.zoneName,
            playTimeSeconds = PlayTimeTracker.Instance.PlayTimeSeconds,
            savedAt = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),

            // ø≠ºË ¿˙¿Â
            ownedKeys = KeyInventory.Instance.GetAllKeys()
        };

        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(SaveDir, $"save_{slotIndex}.json");
        File.WriteAllText(path, json);
    }

    public SaveData Load(int slotIndex)
    {
        string path = Path.Combine(SaveDir, $"save_{slotIndex}.json");
        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }
}

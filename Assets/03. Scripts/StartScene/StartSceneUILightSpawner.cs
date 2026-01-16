using UnityEngine;

public class StartSceneUILightSpawner : MonoBehaviour
{
    [Header("Spawn Area (하단 영역)")]
    [SerializeField] private RectTransform spawnArea;

    [Header("Pool ID")]
    [SerializeField] private string poolId = "StartScene_Light";

    [Header("Wave")]
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private int spawnCountPerWave = 10;

    private float timer;

    private void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        for (int i = 0; i < spawnCountPerWave; i++)
        {
            GameObject obj = PoolManager.Instance.Spawn(poolId);
            if (obj == null) return;

            RectTransform rt = obj.GetComponent<RectTransform>();

            // spawnArea 기준으로 부모 설정
            rt.SetParent(spawnArea, false);

            // spawnArea 로컬 좌표 기준 랜덤 위치
            rt.anchoredPosition = GetRandomPositionInArea();
        }
    }

    private Vector2 GetRandomPositionInArea()
    {
        float w = spawnArea.rect.width;
        float h = spawnArea.rect.height;

        float x = Random.Range(-w * 0.5f, w * 0.5f);
        float y = Random.Range(-h * 0.5f, h * 0.5f);

        return new Vector2(x, y);
    }
}

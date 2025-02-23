using UnityEngine;
using System.Collections.Generic;

public class PoisonCloudManager : MonoBehaviour
{
    public GameObject poisonCloudPrefab; // **毒霧預製體**
    private static PoisonCloudManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // **⚠️ 這個方法將由 `SnakeAI` 調用**
    public static void SpawnPoisonCloud(Vector2 position)
    {
        if (instance != null && instance.poisonCloudPrefab != null)
        {
            Instantiate(instance.poisonCloudPrefab, position, Quaternion.identity);
            Debug.Log($"☠️ 生成毒霧於 {position}");
        }
        else
        {
            Debug.LogError("❌ `PoisonCloudManager` 未正確設置！");
        }
    }
}
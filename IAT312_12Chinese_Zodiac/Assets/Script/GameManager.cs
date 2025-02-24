using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int runesCollected = 0;
    private int totalRunesRequired = 4; // ✅ **四個生肖符文**

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectRune()
    {
        runesCollected++;
        Debug.Log($"✨ 收集了 {runesCollected} / {totalRunesRequired} 符文");
    }

    public bool HasCollectedAllRunes()
    {
        return runesCollected >= totalRunesRequired;
    }

    public void CompleteLevel()
    {
        Debug.Log("🏆 關卡完成！");
    }
}
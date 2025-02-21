using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int completedLevels = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 让 GameManager 在所有场景中保持不被销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteLevel()
    {
        completedLevels++;
        Debug.Log($"🏆 关卡完成，当前已完成 {completedLevels}/4");
    }
}
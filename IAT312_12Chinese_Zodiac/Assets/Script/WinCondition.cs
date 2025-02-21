using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public string nextScene = "LevelSelection";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎉 关卡完成！");
            GameManager.instance.CompleteLevel();
            SceneManager.LoadScene(nextScene);
        }
    }
}
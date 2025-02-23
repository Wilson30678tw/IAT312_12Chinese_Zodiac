using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public string nextScene; // ✅ **每一關對應的下一個場景**
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log($"🎉 觸碰符文，進入 {nextScene}！");
            SceneManager.LoadScene("LevelSelect");
        }
    }
}
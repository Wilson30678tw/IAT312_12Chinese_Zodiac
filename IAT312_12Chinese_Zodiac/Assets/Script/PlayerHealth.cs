using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject gameOverUI; // 玩家死亡時的 UI
    public Slider healthSlider;
    public TMP_Text healthText;
    public float fallDeathThreshold = -10f;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth; // 初始同步血條
        }
        gameOverUI.SetActive(false); // 確保遊戲開始時 UI 隱藏
        Time.timeScale = 1f;
    }
    void Update()
    {
        // ✅ **檢查玩家是否掉出地圖**
        if (transform.position.y < fallDeathThreshold)
        {
            Debug.Log("💀 玩家掉出地圖，死亡！");
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 確保血量不低於 0

        Debug.Log($"🔥 敵人受到 {damage} 傷害！剩餘血量：{currentHealth}");

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // 更新血條
        }
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("💀 玩家死亡！");
        Time.timeScale = 0f; // 暫停遊戲
        gameOverUI.SetActive(true);
    }

    public void Respawn()
    {
        Time.timeScale = 1f; // 恢復遊戲
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新載入關卡
    }

    public void ReturnToLevelSelect()
    {
        Time.timeScale = 1f; // 恢復遊戲
        SceneManager.LoadScene("LevelSelect");
    }
}
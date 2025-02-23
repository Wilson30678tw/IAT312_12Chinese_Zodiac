using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [Header("BOSS 屬性")]
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider;

    [Header("對話選擇 UI")]
    public GameObject choicePanel; // **選擇面板 (內含對話 & 按鈕)**
    public TextMeshProUGUI dialogueText; // **對話框**
    public Button forgiveButton; // **原諒按鈕**
    public Button killButton; // **擊殺按鈕**

    private bool isChoosing = false; // **是否正在對話選擇**

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        // **確保面板 & 按鈕預設關閉**
        if (choicePanel != null) choicePanel.SetActive(false);

        forgiveButton.onClick.AddListener(ChooseForgive);
        killButton.onClick.AddListener(ChooseKill);
    }

    public void TakeDamage(int damage)
    {
        if (isChoosing) return; // **如果正在選擇，則不受傷害**

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 10 && !isChoosing)
        {
            TriggerDialogue(); // **觸發對話**
        }
    }

    void TriggerDialogue()
    {
        isChoosing = true;
        Time.timeScale = 0f; // **暫停遊戲**
        choicePanel.SetActive(true); // **顯示對話 & 按鈕**
        dialogueText.text = "This battle is meaningless...\nAre you really going to end my life?\n Make your choice.";
    }

    public void ChooseForgive()
    {
        Debug.Log("🟢 玩家選擇原諒 BOSS");
        Time.timeScale = 1f; // **恢復遊戲**
        SceneManager.LoadScene("Goodending");
    }

    public void ChooseKill()
    {
        Debug.Log("🔴 玩家選擇擊殺 BOSS");
        Time.timeScale = 1f; // **恢復遊戲**
        SceneManager.LoadScene("Badending");
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }
}
using UnityEngine;

public class LevelGate : MonoBehaviour
{
    public GameObject enterTextUI; // ✅ "按 E 進入" 提示
    public GameObject dialoguePanel; // ✅ 綁定 `DialogueBox`
    public DialogueSystem dialogueSystem; // ✅ 綁定 `DialogueSystem.cs`

    private bool playerInRange = false;

    void Start()
    {
        if (enterTextUI == null)
        {
            Debug.LogError("❌ enterTextUI 未綁定！請在 `Inspector` 手動綁定！");
        }
        if (dialoguePanel == null)
        {
            Debug.LogError("❌ dialoguePanel 未綁定！請在 `Inspector` 手動綁定！");
        }
        if (dialogueSystem == null)
        {
            Debug.LogError("❌ dialogueSystem 未綁定！請在 `Inspector` 手動綁定！");
        }

        enterTextUI.SetActive(false); // ✅ 預設隱藏 "按 E 進入" 提示
        dialoguePanel.SetActive(false); // ✅ 預設隱藏對話框
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("🎬 開始對話！"); // ✅ 記錄 Debug
            dialoguePanel.SetActive(true); // ✅ 顯示對話框
            dialogueSystem.StartDialogue();
            enterTextUI.SetActive(false); // ✅ 隱藏 "按 E 進入" 提示
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎯 玩家靠近門，顯示 `EnterTextUI`");
            playerInRange = true;
            enterTextUI.SetActive(true); // ✅ 顯示 "按 E 進入"
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🚪 玩家離開門，隱藏 `EnterTextUI`");
            playerInRange = false;
            enterTextUI.SetActive(false); // ✅ 離開門時隱藏
            dialoguePanel.SetActive(false); // ✅ 關閉對話框
        }
    }
}
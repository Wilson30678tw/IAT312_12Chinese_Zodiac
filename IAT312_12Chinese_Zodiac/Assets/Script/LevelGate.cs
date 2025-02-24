using UnityEngine;

public class LevelGate : MonoBehaviour
{
    public DialogueSystem dialogueSystem; 
    public string levelToLoad; // ✅ **手動設定場景名稱**
    private bool playerInRange = false; // ✅ **追蹤玩家是否在門的範圍內**
    private bool hasStartedDialogue = false; // ✅ **確保對話不會重複觸發**

    void Start()
    {
        if (dialogueSystem == null)
        {
            dialogueSystem = FindFirstObjectByType<DialogueSystem>();
            if (dialogueSystem == null)
            {
                Debug.LogError("❌ `DialogueSystem` 未找到！請確保場景內有 `DialogueSystem` 物件！");
                return;
            }
        }

        dialogueSystem.levelToLoad = levelToLoad; // ✅ **將 LevelGate 的目標場景傳給 DialogueSystem**
    }

    void Update()
    {
        // ✅ **按下 E 鍵時，只有在門內範圍內才啟動對話**
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasStartedDialogue)
        {
            hasStartedDialogue = true;
            dialogueSystem.StartDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // ✅ **玩家進入範圍**
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // ✅ **玩家離開範圍**
            dialogueSystem.EndDialogue(); // ✅ **離開門時關閉對話**
            hasStartedDialogue = false; // ✅ **允許重新進入時觸發對話**
        }
    }
}
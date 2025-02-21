using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGate : MonoBehaviour
{
    public DialogueSystem dialogueSystem; // 連結對話系統
    public string levelToLoad = "Goat"; // 關卡名稱，預設為 "Goat"
    private bool playerInRange = false;
    private bool hasStartedDialogue = false; // 追蹤是否已開始對話
    private bool isProcessingInput = false; // 防止單幀內多次處理 E 鍵
    private float inputCooldown = 1.0f; // 增加冷卻時間到 1.0 秒，防止快速觸發
    private float lastInputTime = -1f; // 記錄最後一次輸入時間
    private bool isEKeyHeld = false; // 追蹤 E 鍵是否被按住

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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // 僅在對話未開始且未結束時啟動對話，且只允許首次觸發
            if (!dialogueSystem.IsDialogueActive && !hasStartedDialogue)
            {
                dialogueSystem.StartDialogue();
                hasStartedDialogue = true; // 標記已開始對話
                Debug.Log("觸發對話開始，currentLine: " + dialogueSystem.CurrentLine);
            }
        }
    }

    void Update()
    {
        if (playerInRange && dialogueSystem.IsDialogueActive)
        {
            float currentTime = Time.time;
            if (Input.GetKeyDown(KeyCode.E) && !isProcessingInput && !isEKeyHeld && (currentTime - lastInputTime >= inputCooldown))
            {
                isProcessingInput = true; // 標記正在處理輸入
                isEKeyHeld = true; // 標記 E 鍵被按住
                lastInputTime = currentTime; // 記錄輸入時間
                Debug.Log("玩家按下 E，currentLine 為: " + dialogueSystem.CurrentLine + ", 時間: " + currentTime);
                dialogueSystem.DisplayNextLine();
            }

            // 重置輸入處理標記
            if (Input.GetKeyUp(KeyCode.E))
            {
                isProcessingInput = false;
                isEKeyHeld = false; // 重置 E 鍵按住狀態
                Debug.Log("E 鍵釋放，準備下次輸入，currentLine: " + dialogueSystem.CurrentLine);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (dialogueSystem.IsDialogueActive)
            {
                dialogueSystem.EndDialogue(); // 結束對話
                hasStartedDialogue = false; // 重置對話開始標記
                Debug.Log("玩家離開觸發區，對話結束，currentLine 重置為: " + dialogueSystem.CurrentLine);
            }
        }
    }
}
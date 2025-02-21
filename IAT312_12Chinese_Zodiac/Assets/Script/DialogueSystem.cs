using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueBox; // ✅ 對話框
    public TextMeshProUGUI dialogueText; // ✅ 顯示對話的文字
    public string[] dialogueLines; // ✅ 對話內容
    private int currentLine = -1; // ✅ 初始為 -1
    private bool isDialogueActive = false; // ✅ 追蹤對話狀態
    private bool isProcessingInput = false; // ✅ 防止 `E` 鍵連續觸發
    private float inputCooldown = 0.5f; // ✅ 冷卻時間，防止 `E` 被連續觸發
    private float lastInputTime = -1f; // ✅ 記錄 `E` 的最後輸入時間
    public string levelToLoad = "Goat"; // ✅ 指定切換場景名稱

    public bool IsDialogueActive => isDialogueActive;
    public int CurrentLine => currentLine;

    void Start()
    {
        if (dialogueBox == null)
        {
            Debug.LogError("❌ dialogueBox 未綁定！請檢查 Inspector 設置！");
        }
        if (dialogueText == null)
        {
            Debug.LogError("❌ dialogueText 未綁定！請檢查 Inspector 設置！");
        }

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false); // ✅ 預設關閉對話框
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && !isProcessingInput && isDialogueActive)
        {
            float currentTime = Time.time;
            if (currentTime - lastInputTime >= inputCooldown)
            {
                isProcessingInput = true;
                currentLine++;
                lastInputTime = currentTime;
                DisplayNextLine();
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isProcessingInput = false;
           
            
        }
    }

    public void StartDialogue()
    {
        
        currentLine = 0; // ✅ 確保第一句能夠正確顯示
        isDialogueActive = true;
        dialogueBox.SetActive(true);
        dialogueText.text = dialogueLines[currentLine];
        Debug.Log("▶️ 對話開始，顯示第 " + currentLine + " 行: " + dialogueLines[currentLine]);
    }

    public void DisplayNextLine()
    {
        if (!isDialogueActive)
        {
           
            Debug.LogWarning("⚠️ 對話未開始，無法顯示下一行！");
            return;
        }
        
        
        Debug.Log("⏩ 切換到第 " + currentLine + " 行");

        if (currentLine < dialogueLines.Length)
        {
            
            dialogueText.text = dialogueLines[currentLine];
            Debug.Log("📝 顯示內容: " + dialogueLines[currentLine]);
            
        }
        else
        {
            Debug.Log("🏁 對話結束，準備切換場景");
            EndDialogue();
        }
    }

    public bool IsDialogueFinished()
    {
        bool finished = currentLine >= dialogueLines.Length - 1;
        Debug.Log("🔍 `IsDialogueFinished()` 被調用，currentLine: " + currentLine + " / " + dialogueLines.Length + "，結果: " + finished);
        return finished;
    }

    public void EndDialogue()
    {
        if (!isDialogueActive) return;

        isDialogueActive = false;
        dialogueBox.SetActive(false);
        Debug.Log("🛑 對話框關閉，currentLine 重置為 -1");

        // ✅ 確保對話結束後才載入新場景
        if (!string.IsNullOrEmpty(levelToLoad))
        {
            Debug.Log("🚀 載入場景：" + levelToLoad);
            SceneManager.LoadScene(levelToLoad);
        }
    }
}

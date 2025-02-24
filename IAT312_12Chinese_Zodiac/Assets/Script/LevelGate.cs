using UnityEngine;

public class LevelGate : MonoBehaviour
{
    public DialogueSystem dialogueSystem; 
    public string levelToLoad; // ✅ **手動設定場景名稱**
    private bool playerInRange = false;
    private bool hasStartedDialogue = false;

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
        
        dialogueSystem.levelToLoad = levelToLoad; // ✅ **將 LevelGate 的目標場景賦值給 DialogueSystem**
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasStartedDialogue)
        {
            hasStartedDialogue = true;
            dialogueSystem.StartDialogue();
        }
    }
}
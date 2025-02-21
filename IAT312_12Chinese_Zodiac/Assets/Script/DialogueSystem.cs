using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueBox; // ✅ 綁定對話框（DialogueBox）
    public TextMeshProUGUI dialogueText; // ✅ 這是顯示對話的 `TextMeshProUGUI`
    public string[] dialogueLines; // ✅ 存儲對話內容
    public string levelToLoad; // ✅ 記錄要加載的關卡名稱

    private int currentLine = 0;
    private bool dialogueActive = false;

    void Start()
    {
        dialogueBox.SetActive(false); // ✅ 確保遊戲開始時對話框隱藏
    }

    public void StartDialogue()
    {
        dialogueActive = true;
        dialogueBox.SetActive(true); // ✅ 顯示對話框
        dialogueText.text = dialogueLines[currentLine]; // ✅ 顯示第一句對話
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextLine();
        }
    }

    void DisplayNextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine]; // ✅ 顯示下一句話
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialogueBox.SetActive(false); // ✅ 關閉對話框
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad); // ✅ 進入關卡
    }
}
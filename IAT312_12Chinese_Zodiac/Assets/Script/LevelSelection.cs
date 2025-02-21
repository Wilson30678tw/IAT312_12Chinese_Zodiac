using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button finalBattleButton; // `FinalBattle` 按钮

    void Start()
    {
        // 🔥 **如果玩家未完成 4 关，禁用 `FinalBattle` 按钮**
        finalBattleButton.interactable = GameManager.instance.completedLevels >= 4;
    }

    public void LoadGoatLevel() { SceneManager.LoadScene("Goat"); }
    public void LoadRoosterLevel() { SceneManager.LoadScene("Rooster"); }
    public void LoadSnakeLevel() { SceneManager.LoadScene("Snake"); }
    public void LoadDragonLevel() { SceneManager.LoadScene("Dragon"); }
    public void LoadFinalBattle() { SceneManager.LoadScene("Boss"); }

    public void BackToMainMenu() { SceneManager.LoadScene("StartMenu"); }
}
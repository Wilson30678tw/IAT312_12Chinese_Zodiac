using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossUnlock : MonoBehaviour
{
    public Button bossButton; // ✅ 進入 Boss 戰鬥的按鈕

    void Start()
    {
        int collectedRunes = 0;
        if (PlayerPrefs.GetInt("GoatRune", 0) == 1) collectedRunes++;
        if (PlayerPrefs.GetInt("RoosterRune", 0) == 1) collectedRunes++;
        if (PlayerPrefs.GetInt("SnakeRune", 0) == 1) collectedRunes++;
        if (PlayerPrefs.GetInt("DragonRune", 0) == 1) collectedRunes++;

        bossButton.interactable = (collectedRunes >= 4); // ✅ 收集 4 個符文才能進入 Boss 關卡
    }

    public void LoadBossLevel()
    {
        SceneManager.LoadScene("Boss");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // 切换到关卡选择页面
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial"); // 进入教程页面
    }

    public void QuitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
}
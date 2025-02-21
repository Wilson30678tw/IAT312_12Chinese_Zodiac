using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("StartMenu"); // 返回主菜单
    }
}
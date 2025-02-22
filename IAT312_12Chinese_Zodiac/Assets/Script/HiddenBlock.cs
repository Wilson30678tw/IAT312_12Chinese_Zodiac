using UnityEngine;

public class HiddenBlock : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false); // 🚀 預設為隱藏
    }

    public void ActivateBlock()
    {
        gameObject.SetActive(true); // ✅ 讓隱藏板塊變為可見 & 可行走
    }
}
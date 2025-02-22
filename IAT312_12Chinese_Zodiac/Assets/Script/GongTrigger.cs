using UnityEngine;

public class GongTrigger : MonoBehaviour
{
    public HiddenBlock hiddenBlock; // ✅ 連接要顯示的隱藏板塊

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InkProjectile")) // 🚀 確保被墨汁擊中
        {
            if (hiddenBlock != null)
            {
                hiddenBlock.ActivateBlock(); // ✅ 讓隱藏板塊顯示
                Debug.Log("🎵 鑼鼓被擊中！隱藏板塊出現！");
            }
            else
            {
                Debug.LogError("❌ HiddenBlock 未連接！請在 Inspector 連接隱藏板塊。");
            }

            Destroy(collision.gameObject); // ✅ 墨汁子彈消失
        }
    }
}
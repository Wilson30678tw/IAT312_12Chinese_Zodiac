using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public string runeName; // ✅ 設定符文名稱（GoatRune、RoosterRune、SnakeRune、DragonRune）

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"🟠 碰到符文：{runeName}");

            if (RuneManager.instance != null)
            {
                RuneManager.instance.CollectRune();
                Destroy(gameObject); // ✅ 符文被收集後消失
            }
        }
    }
}
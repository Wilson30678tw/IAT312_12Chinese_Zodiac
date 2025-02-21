using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // 最大生命值
    private int currentHealth; // 当前生命值

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"❤️ 玩家受到了 {damage} 伤害，剩余血量：{currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("💀 玩家死亡！");
        // 你可以添加游戏失败逻辑，例如重置关卡
    }
}
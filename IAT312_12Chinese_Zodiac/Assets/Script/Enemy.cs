using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 20; // 最大血量
    private int currentHealth; // 当前血量
    public float speed = 2f; // 移动速度
    public int attackDamage = 5; // 攻击伤害
    public float attackRange = 1f; // 攻击范围
    public float attackCooldown = 2f; // 攻击冷却时间
    private float lastAttackTime;

    private Transform player; // 玩家目标
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // 查找玩家
    }

    void Update()
    {
        if (player == null) return; // 如果玩家不存在，则不执行下面的逻辑

        // **简单 AI 追踪玩家**
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            // **移动向玩家**
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Time.time - lastAttackTime > attackCooldown)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        Debug.Log("⚔️ 敌人攻击玩家！");
        lastAttackTime = Time.time;

        // **假设玩家有 `PlayerHealth` 组件**
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"💥 敌人受到了 {damage} 伤害，剩余血量：{currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("💀 敌人死亡！");
        Destroy(gameObject);
    }
}
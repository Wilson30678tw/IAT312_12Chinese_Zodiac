using UnityEngine;
using System.Collections;

public class EnergyBall : MonoBehaviour
{
    public float speed = 10f; // ✅ 提高元氣彈速度
    public int damage = 15; // ✅ 增加傷害值
    public float lifetime = 3f; // 元氣彈存在時間
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("❌ EnergyBall 缺少 Rigidbody2D！");
            return;
        }

        StartCoroutine(AutoDestroy());
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(lifetime);
        Debug.Log("🔥 元氣彈消失！");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"💥 元氣彈擊中 {collision.gameObject.name}");

        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"💥 元氣彈對玩家造成 {damage} 點傷害！");
            }
            Destroy(gameObject);
        }
        else if (((1 << collision.gameObject.layer) & groundLayer) != 0) // ✅ 使用 Layer 來判斷地面
        {
            Debug.Log("🧱 元氣彈擊中地面（Layer），銷毀！");
            Destroy(gameObject);
        }
    }
}
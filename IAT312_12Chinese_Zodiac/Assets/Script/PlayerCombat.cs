using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GameObject inkProjectilePrefab; // 墨汁子弹预制体
    public Transform firePoint; // 发射点
    public float inkSpeed = 10f; // 墨汁飞行速度

    void Update()
    {
        // 按 `Space` 键发射墨汁
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootInk();
        }
    }

    void ShootInk()
    {
        // 生成墨汁投射物
        GameObject ink = Instantiate(inkProjectilePrefab, firePoint.position, firePoint.rotation);
        
        // 让墨汁向前飞行
        Rigidbody2D rb = ink.GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.localScale.x * Vector2.right * inkSpeed;
    }
}
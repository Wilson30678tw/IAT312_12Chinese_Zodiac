using UnityEngine;
using System.Collections;

public class GoatAI : MonoBehaviour
{
    public float patrolSpeed = 2f;  // 羊巡邏速度
    public float chargeSpeed = 10f;  // 衝撞速度
    public float chargeDuration = 1f; // 衝撞持續時間
    public int damage = 5; // 衝撞造成的傷害
    public float knockbackForce = 15f; // 擊退力度（水平推力）
    public float knockbackYMultiplier = 1.5f; // Y 軸擊退倍率

    private Rigidbody2D rb;
    private bool movingRight = true;
    private bool isCharging = false;
    private float chargeEndTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // 每 3-5 秒隨機翻轉方向
        InvokeRepeating("Flip", Random.Range(3f, 5f), Random.Range(3f, 5f));
    }

    void Update()
    {
        if (isCharging)
        {
            if (Time.time > chargeEndTime)
            {
                isCharging = false;
                rb.linearVelocity = Vector2.zero; 
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (!isCharging)
        {
            rb.linearVelocity = new Vector2(movingRight ? patrolSpeed : -patrolSpeed, rb.linearVelocity.y);
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        Debug.Log("Flipped! New direction: " + (movingRight ? "Right" : "Left"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected! Charging attack.");
            ChargeAtPlayer(collision.gameObject);
        }
    }

    void ChargeAtPlayer(GameObject player)
    {
        if (!isCharging)
        {
            isCharging = true;
            chargeEndTime = Time.time + chargeDuration;

            float direction = (player.transform.position.x > transform.position.x) ? 1f : -1f;
            rb.linearVelocity = new Vector2(chargeSpeed * direction, rb.linearVelocity.y);

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            PlayerController playerController = player.GetComponent<PlayerController>(); // 獲取玩家控制腳本
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            if (playerRb != null)
            {
                // **禁用玩家控制**
                if (playerController != null)
                {
                    playerController.enabled = false; // 關閉玩家控制
                }

                // **施加擊退**
                Vector2 knockback = new Vector2(direction * knockbackForce, knockbackForce * knockbackYMultiplier);
                playerRb.linearVelocity = knockback;
                Debug.Log("Player knocked back with force: " + knockback);

                // **重新啟用玩家控制（0.5 秒後）**
                StartCoroutine(EnablePlayerControl(playerController, 0.5f));
            }
        }
    }

    IEnumerator EnablePlayerControl(PlayerController playerController, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerController != null)
        {
            playerController.enabled = true; // 恢復玩家控制
        }
    }
}

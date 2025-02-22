using UnityEngine;
using System.Collections;

public class BlockControl : MonoBehaviour
{
    public float moveSpeed = 2f; // 平台移動速度
    public float moveDistance = 3f; // 平台移動範圍（來回）

    private Vector3 startPos;
    private int direction = 1;
    private bool isFallingPlatform = false; // 是否為裂縫地板
    private bool isMovingUp = false; // 是否為上下移動地板
    private bool isMovingLeftRight = false; // 是否為左右移動地板

    void Start()
    {
        startPos = transform.position;

        // ✅ 根據 Layer 設定不同的移動模式
        if (gameObject.layer == LayerMask.NameToLayer("Falling Ground"))
        {
            isFallingPlatform = true;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Moving updGround"))
        {
            isMovingUp = true;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Moving LeftRGround"))
        {
            isMovingLeftRight = true;
        }
    }

    void Update()
    {
        if (isFallingPlatform) return; // ✅ 如果是裂縫地板，不進行移動

        if (isMovingUp)
        {
            // ✅ 上下移動平台
            transform.position += new Vector3(0, direction * moveSpeed * Time.deltaTime, 0);
        }
        else if (isMovingLeftRight)
        {
            // ✅ 左右移動平台
            transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
        }

        // ✅ 檢查移動範圍，達到範圍後反向
        if (Vector3.Distance(startPos, transform.position) >= moveDistance)
        {
            direction *= -1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ✅ 處理裂縫地板
        if (isFallingPlatform && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallPlatform());
        }
    }

    IEnumerator FallPlatform()
    {
        yield return new WaitForSeconds(2f); // ✅ 玩家踩上後 2 秒掉落
        GetComponent<Collider2D>().enabled = false; // ✅ 關閉碰撞

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // ✅ 讓地板掉落
            rb.gravityScale = 1f;
        }

        yield return new WaitForSeconds(1f);
        Destroy(gameObject); // ✅ 1 秒後完全刪除地面
    }
}

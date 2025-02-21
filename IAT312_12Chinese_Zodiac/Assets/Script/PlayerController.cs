using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // 移动速度
    public float jumpForce = 10f; // 跳跃力度
    public int maxJumps = 2; // 最大跳跃次数

    private Rigidbody2D rb;
    private int jumpCount; // 当前跳跃次数
    private bool isGrounded; // 是否站在地面上
    private SpriteRenderer spriteRenderer; // 角色的 SpriteRenderer

    public Transform groundCheck; // 地面检测点
    public LayerMask groundLayer; // 地面层
    public Transform firePoint; // FirePoint 变量
    private bool facingRight = true; // 角色当前面朝的方向

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 初始化 SpriteRenderer
        jumpCount = maxJumps; // 初始跳跃次数

        if (firePoint == null)
        {
            Debug.LogError("❌ FirePoint 未绑定！请在 Unity Inspector 里手动拖入 FirePoint 物体！");
        }
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");

        // ✅ **使用 `linearVelocity` 代替 `velocity`**
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // **🔥 让角色正确翻转（仅翻转 X 轴，不改变大小）**
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        // **跳跃**
        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
        }
    }

    void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // 角色落地时重置跳跃次数
        if (!wasGrounded && isGrounded)
        {
            jumpCount = maxJumps;
        }
    }

    // 🔥 **只翻转 X 轴，不改变大小**
    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        // 🔥 **同步 FirePoint 位置**
        if (firePoint != null)
        {
            firePoint.localPosition = new Vector3(-firePoint.localPosition.x, firePoint.localPosition.y, 0);
        }
    }
}

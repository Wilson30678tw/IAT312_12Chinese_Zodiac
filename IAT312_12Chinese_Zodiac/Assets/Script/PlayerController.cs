using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;          // 移动速度
    public float jumpForce = 10f;     // 跳跃力度
    public int maxJumps = 2;          // 最大跳跃次数

    private Rigidbody2D rb;
    private int jumpCount;            // 当前跳跃次数
    private bool isGrounded;          // 是否站在地面上

    public Transform groundCheck;     // 地面检测点
    public LayerMask groundLayer;     // 地面层

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;         // 初始跳跃次数
    }

    void Update()
    {
        // 左右移动
        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // 改为按 `↑` 方向键跳跃
        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;  // 消耗一次跳跃机会
        }
    }

    // 通过射线检测地面
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
}
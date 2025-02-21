using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using System.Collections;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("基本屬性")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 1;

    private Rigidbody2D rb;
    private int jumpCount;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;

    [Header("地面檢測")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("夜視模式（雞關卡）")]
    public Light2D globalLight;
    public float nightVisionDuration = 5f;
    private float nightVisionCooldown = 0f;
    private bool canUseNightVision = false;
    private bool isNightVisionActive = false;

    [Header("蛇關卡 - Dash（地面 & 空中衝刺）")]
    private bool canDash = false;
    private bool isDashing = false;
    private float dashCooldown = 0f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;

    [Header("龍關卡 - 飛行能力")]
    private bool canFly = false;
    private bool isFlying = false;
    public float flySpeed = 5f;
    public float maxFlyHeight = 10f;

    [Header("UI 冷卻顯示")]
    public Image nightVisionIcon;
    public TMP_Text nightVisionCooldownText;
    public Image dashIcon;
    public TMP_Text dashCooldownText;

    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpCount = maxJumps;

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Rooster")
        {
            globalLight.intensity = 0.005f;
            canUseNightVision = true;
        }
        else if (sceneName == "Goat")
        {
            maxJumps = 2;
        }
        else if (sceneName == "Snake")
        {
            canDash = true;
        }
        else if (sceneName == "Dragon")
        { 
            canFly = true;
        }
        else if (sceneName == "Boss" || sceneName == "Tutorial")
        {
            canDash = true;
            canUseNightVision = true;
            maxJumps = 2;
            canFly = true; 
        }

        if (globalLight == null)
        {
            globalLight = Object.FindAnyObjectByType<Light2D>(); 

        }
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");

        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        }

        if (move > 0 && !facingRight) Flip();
        else if (move < 0 && facingRight) Flip();

        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
        }

        // ✅ **夜視技能冷卻 & 觸發**
        if (canUseNightVision && Input.GetKeyDown(KeyCode.N) && nightVisionCooldown <= 0)
        {
            ToggleNightVision();
        }

        if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && dashCooldown <= 0)
        {
            StartCoroutine(Dash());
        }

        if (canFly && Input.GetKey(KeyCode.LeftControl))
        {
            StartFlying();
        }
        else
        {
            StopFlying();
        }

        // ✅ **確保冷卻時間減少**
        if (nightVisionCooldown > 0)
        {
            nightVisionCooldown -= Time.deltaTime;
        }
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }

        UpdateCooldownUI();
    }

    void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (!wasGrounded && isGrounded)
        {
            jumpCount = maxJumps;
        }

        if (isFlying)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flySpeed);
        }
    }

    // ✅ **夜視模式開關**
    void ToggleNightVision()
    {
        if (!isNightVisionActive)
        {
            isNightVisionActive = true;
            globalLight.intensity = 1.5f;
            globalLight.color = new Color(1f, 1f, 0.8f);
            nightVisionCooldown = nightVisionDuration + 3f; // ✅ 設定冷卻時間（夜視時間 + 3 秒冷卻）
            StartCoroutine(NightVisionTimer());
        }
    }
    public float GetNightVisionCooldown()
    {
        return nightVisionCooldown; // ✅ 允許外部腳本讀取 `nightVisionCooldown`
    }

    // ✅ **夜視模式 5 秒後關閉**
    IEnumerator NightVisionTimer()
    {
        yield return new WaitForSeconds(nightVisionDuration);
        globalLight.intensity = 0.005f;
        globalLight.color = Color.white;
        isNightVisionActive = false;
    }

    // ✅ **`Shift` 觸發 Dash（地面 & 空中）**
    IEnumerator Dash()
    {
        isDashing = true;
        dashCooldown = 1.5f;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float dashDirection = facingRight ? 1f : -1f;

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(dashDirection * (dashSpeed * 0.8f), rb.linearVelocity.y * 0.5f);
        }

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    // ✅ **龍關卡飛行**
    void StartFlying()
    {
        isFlying = true;
        rb.gravityScale = 0.2f;
        if (transform.position.y < maxFlyHeight) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flySpeed);
        }
    }

    void StopFlying()
    {
        isFlying = false;
        rb.gravityScale = 1f;
    }

    void UpdateCooldownUI()
    {
        UpdateSkillUI(nightVisionIcon, nightVisionCooldownText, ref nightVisionCooldown);
        UpdateSkillUI(dashIcon, dashCooldownText, ref dashCooldown);
    }

    void UpdateSkillUI(Image icon, TMP_Text cooldownText, ref float cooldown)
    {
        if (cooldown > 0)
        {
            cooldownText.text = Mathf.Ceil(cooldown).ToString();
            icon.color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            cooldownText.text = "";
            icon.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}

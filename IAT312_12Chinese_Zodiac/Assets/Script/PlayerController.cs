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
    private Transform currentPlatform = null;
    private Vector3 lastPlatformPosition;

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

    [Header("UI 冷卻顯示")]
    public Image nightVisionIcon;
    public TMP_Text nightVisionCooldownText;
    public Image dashIcon;
    public TMP_Text dashCooldownText;
    public Image flyIcon;
    public TMP_Text flyCooldownText;
    public float GetNightVisionCooldown()
    {
        return nightVisionCooldown;  // ✅ 返回當前的夜視冷卻時間
    }

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

        if (canUseNightVision && Input.GetKeyDown(KeyCode.N) && nightVisionCooldown <= 0)
        {
            ToggleNightVision();
        }

        if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && dashCooldown <= 0)
        {
            StartCoroutine(Dash());
        }
        // ✅ **第一次按 `LeftControl` 時，啟動飛行並開始 5 秒倒數**
        // ✅ **飛行系統**
        if (canFly)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                StartFlying();
            }
            else
            {
                StopFlying();
            }
        }




        if (nightVisionCooldown > 0) nightVisionCooldown -= Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;

        UpdateCooldownUI();
    }

    void FixedUpdate()
    {
              // ✅ **按住 LeftControl 時上升**
              if (isFlying && Input.GetKey(KeyCode.LeftControl))
              {
                  rb.linearVelocity = new Vector2(rb.linearVelocity.x, flySpeed);
              }
        

        if (currentPlatform != null)
        {
            Vector3 platformMovement = currentPlatform.position - lastPlatformPosition;
            transform.position += platformMovement;
            lastPlatformPosition = currentPlatform.position;
        }
    }

    void ToggleNightVision()
    {
        if (!isNightVisionActive)
        {
            isNightVisionActive = true;
            globalLight.intensity = 1.5f;
            globalLight.color = new Color(1f, 1f, 0.8f);
            nightVisionCooldown = nightVisionDuration + 3f;
            StartCoroutine(NightVisionTimer());
        }
    }

    IEnumerator NightVisionTimer()
    {
        yield return new WaitForSeconds(nightVisionDuration);
        globalLight.intensity = 0.005f;
        globalLight.color = Color.white;
        isNightVisionActive = false;
    }

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

    void StartFlying()
    {
        isFlying = true;
        rb.gravityScale = 0.2f; // ✅ 減少重力影響
    }

    void StopFlying()
    {
        isFlying = false;
        rb.gravityScale = 1f; // ✅ 恢復正常重力
    }

    void UpdateCooldownUI()
    {
        UpdateSkillUI(nightVisionIcon, nightVisionCooldownText, ref nightVisionCooldown);
        UpdateSkillUI(dashIcon, dashCooldownText, ref dashCooldown);
        //UpdateSkillUI(flyIcon, flyCooldownText, ref flyCooldownTimer);
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

using UnityEngine;
using System.Collections;

public class PoisonCloud : MonoBehaviour
{
    public int poisonDamage = 10; // 每秒毒霧傷害
    public float damageInterval = 1f; // 傷害間隔
    private bool playerInside = false; // **檢測玩家是否在毒霧範圍內**

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            StartCoroutine(ApplyPoisonDamage(collision.GetComponent<PlayerHealth>()));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    IEnumerator ApplyPoisonDamage(PlayerHealth playerHealth)
    {
        while (playerInside && playerHealth != null)
        {
            playerHealth.TakeDamage(poisonDamage);
            Debug.Log($"☠️ 玩家受到 {poisonDamage} 點毒霧傷害！");
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
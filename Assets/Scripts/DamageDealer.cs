using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }

            // Destroy bullet or damage source
            Destroy(gameObject);
        }
    }
}

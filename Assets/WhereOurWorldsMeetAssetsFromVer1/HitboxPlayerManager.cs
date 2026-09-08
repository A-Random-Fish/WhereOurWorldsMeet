using UnityEngine;
using System.Collections;

public class HitboxPlayerManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] int damage;
    EnemyHealthComponent enemyHealth;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyHealth = other.GetComponent<EnemyHealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(damage);
                enemyHealth.StartCoroutine(enemyHealth.KnockbackEnemy(player.transform));
                player.GetComponent<PlayerController>().HitKBFunc();
            }
        }   
    }
}

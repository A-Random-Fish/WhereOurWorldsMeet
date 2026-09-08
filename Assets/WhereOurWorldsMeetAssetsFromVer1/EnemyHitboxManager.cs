using UnityEngine;

public class EnemyHitboxManager : MonoBehaviour
{
    [SerializeField] int damageToDeal;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.GetComponent<PlayerHealthComponent>() != null)
            {
                PlayerHealthComponent phc = col.GetComponent<PlayerHealthComponent>();
                if (phc.iframes <= 0)
                    phc.StartCoroutine(phc.KnockbackPlayer(transform));
                    
                phc.PlayerDamage(damageToDeal);
            }
            
        }
    }
}

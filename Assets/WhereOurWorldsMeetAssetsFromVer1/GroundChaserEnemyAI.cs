using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Timeline;

public class GroundChaserEnemyAI : MonoBehaviour
{
    NavMeshAgent nma;
    Animator anim;
    GameObject[] players;
    GameObject target;
    float smallestDistance;
    [SerializeField] float stoppingDistance;
    [SerializeField] float enemySpeed;
    float attackCooldown = 1f;
    [SerializeField] float attackCooldownDuration;
    [SerializeField] GameObject HitboxGameObject;
    [SerializeField] float sightRange;
    bool attacking;
    EnemyHealthComponent ehc;

    void Start()
    {
        HitboxGameObject.GetComponent<Collider>().enabled = false;
        nma = GetComponent<NavMeshAgent>();
        players = GameObject.FindGameObjectsWithTag("Player");
        anim = GetComponent<Animator>();
        ehc = GetComponent<EnemyHealthComponent>();
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;
        //get nearest player, set to target, move to target
        smallestDistance = 99999999f;
        if (players.Length > 1)
        {
            for (int i = 0; i < players.Length -1; i++)
            {
                float distance = Vector3.Distance(players[i].transform.position, transform.position);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    if (distance <= sightRange)
                        target = players[i];
                    else
                        target = gameObject;
                }
            }
        }
        else if (Vector3.Distance(players[0].transform.position, transform.position) <= sightRange && ehc.health > 0)
        {
            target = players[0];
        }
        else
        {
            target = gameObject;
        }

        if (target != null && nma.enabled || ehc.health <= 0 && nma.enabled)
            nma.destination = target.transform.position;

        if (Vector3.Distance(target.transform.position, transform.position) <= stoppingDistance && target != gameObject && ehc.health > 0) 
        {
            nma.speed = 0f;
            if (attackCooldown <= 0f)
            {
                attackCooldown = attackCooldownDuration;
                StartCoroutine("AttackHitboxEnable");
            }
        }
        else if (!attacking && ehc.health > 0)
        {
            nma.speed = enemySpeed;
        }

        anim.SetBool("Moving", nma.enabled == true && nma.hasPath);
        anim.SetBool("Attacking", attacking);
        
    }

    private IEnumerator AttackHitboxEnable()
    {
        attacking = true;
        nma.speed = 0;
        yield return new WaitForSeconds(0.5f);
        HitboxGameObject.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        HitboxGameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        attacking = false;
        nma.speed = enemySpeed;
    }
}

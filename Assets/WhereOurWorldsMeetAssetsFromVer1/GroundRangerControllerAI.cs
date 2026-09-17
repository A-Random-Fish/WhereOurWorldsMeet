using UnityEngine;
using UnityEngine.AI;

public class GroundRangerControllerAI : MonoBehaviour
{
    NavMeshAgent nma;
    [SerializeField] float moveSpeed;
    [SerializeField] float attackCooldown;
    [SerializeField] float sightRange;
    GameObject[] players;
    float smallestDistance;
    GameObject target;
    EnemyHealthComponent ehc;
    Vector3 finalPosition;

    [SerializeField] float walkRadius;

    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        ehc = GetComponent<EnemyHealthComponent>();
        players = GameObject.FindGameObjectsWithTag("Player");
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
                    if (distance <= sightRange && ehc.health > 0)
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
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;
        nma.destination = finalPosition;

        //get a point around the currently targeted player, without needing to run around the player in order to pathfind, and make sure it is still an available navmesh. point, shoot, pause, pathfind again.
    }
}

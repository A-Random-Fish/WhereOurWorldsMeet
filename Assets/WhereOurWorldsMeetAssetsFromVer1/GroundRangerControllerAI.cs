using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GroundRangerControllerAI : MonoBehaviour
{
    NavMeshAgent nma;
    [SerializeField] float attackCooldown;
    [SerializeField] float sightRange;
    GameObject[] players;
    float smallestDistance;
    GameObject target;
    EnemyHealthComponent ehc;
    Vector3 finalPosition;
    bool pathFinding = false;
    [SerializeField] float walkingCooldown;

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
                    if (distance > sightRange && ehc.health > 0)
                        target = players[i];
                    else
                        target = gameObject;
                }
            }
        }
        else if (Vector3.Distance(players[0].transform.position, transform.position) > sightRange && ehc.health > 0)
        {
            target = players[0];
        }
        else
        {
            target = gameObject;
        }

        // Starts maneuvering like snake my solid
        if (!pathFinding && Vector3.Distance(target.transform.position, transform.position) <= sightRange)
        {
            StartCoroutine("RangerWalking");

        }
        if (target != null && !pathFinding)
        {    
            nma.destination = target.transform.position;
        }

        //get a point around the currently targeted player, without needing to run around the player in order to pathfind, and make sure it is still an available navmesh. point, shoot, pause, pathfind again.
    }

    IEnumerator RangerWalking()
    {
        Debug.Log("Works");
        pathFinding = true;
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius; // 'Random.insideUnitSphere' generates a random vector3, and multiplies this with the walk radius to see how far it can go.
        randomDirection += transform.position; // Stupid way of walking, I wont fix this.
        NavMeshHit hit; 
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas); 
        Vector3 finalPosition = hit.position;
        nma.destination = finalPosition;
        yield return new WaitForSeconds(walkingCooldown);
        pathFinding = false;
    }
}

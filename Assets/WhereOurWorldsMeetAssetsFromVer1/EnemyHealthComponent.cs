using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyHealthComponent : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    public int health;
    [SerializeField] float iframes;
    [SerializeField] int maxHealth;
    NavMeshAgent nma;
    [SerializeField] int defense;
    [SerializeField] float kbDuration;
    [SerializeField] float kbForce;
    [SerializeField] List<GameObject> bodyObjects;
    [SerializeField] Material normalMat;
    [SerializeField] Material hitFlashMat;

    bool dead;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        nma = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        iframes -= Time.deltaTime;
    }

    public void Damage(int damage)
    {
        if (iframes <= 0 && health > 0)
        {
            int damageToDeal = damage - defense;
            health -= damageToDeal;
            iframes = 0.33f;
            StartCoroutine("HitFlash");
            
            if (health <= 0 && !dead)
            {
                dead = true;
                StartCoroutine("Death");
            }
        }
    }

    public IEnumerator KnockbackEnemy(Transform kbLocation)
    {
        if (health > 0)
        {
            nma.enabled = false;
            rb.isKinematic = false;
            Vector3 kbDir = transform.position - kbLocation.position;
            kbDir = new Vector3(kbDir.x, 0, kbDir.z);
            rb.AddForce(kbDir.normalized * kbForce, ForceMode.Impulse);
            yield return new WaitForSeconds(kbDuration);
            nma.Warp(transform.position);
            nma.enabled = true;
            rb.isKinematic = true;
        }
    }

    private IEnumerator HitFlash()
    {
        for (int i = 0; i < bodyObjects.Count; i++)
        {
            if (bodyObjects[i].GetComponent<MeshRenderer>() != null)
                bodyObjects[i].GetComponent<MeshRenderer>().material = hitFlashMat;
            else if (bodyObjects[i].GetComponent<SkinnedMeshRenderer>() != null)
                bodyObjects[i].GetComponent<SkinnedMeshRenderer>().material = hitFlashMat;
        }

        yield return new WaitForSeconds(0.15f);

        for (int i = 0; i < bodyObjects.Count; i++)
        {
            if (bodyObjects[i].GetComponent<MeshRenderer>() != null)
                bodyObjects[i].GetComponent<MeshRenderer>().material = normalMat;
            else if (bodyObjects[i].GetComponent<SkinnedMeshRenderer>() != null)
                bodyObjects[i].GetComponent<SkinnedMeshRenderer>().material = normalMat;
        }
    }

    private IEnumerator Death()
    {
        anim.Play("windGolem1Dead");
        nma.enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }


}

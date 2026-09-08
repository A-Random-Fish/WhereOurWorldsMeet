using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealthComponent : MonoBehaviour
{ 
    Rigidbody rb;
    [SerializeField] int health;
    PlayerController pc;

    [SerializeField] List<GameObject> bodyObjects;
    [SerializeField] Material normalMat;
    [SerializeField] Material hitFlashMat;

    [SerializeField] float kbForce;
    public float iframes;
    [SerializeField] int maxHealth;
    [SerializeField] int defense;
    [SerializeField] float kbDuration;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        pc = GetComponent<PlayerController>();
    }

    public void PlayerDamage(int damage)
    {
        if (iframes <= 0)
        {
            int damageToDeal = damage - defense;
            health -= damageToDeal;
            iframes = 0.5f;
            StartCoroutine("HitFlash");
            if (health <= 0 )
            {
                Death();
            }
        }
    }

    void Update()
    {
        iframes -= Time.deltaTime;
    }

    public IEnumerator KnockbackPlayer(Transform kbLocation)
    {
        pc.canMove = false;
        Vector3 kbDir = transform.position - kbLocation.position;
        rb.AddForce(kbDir.normalized * kbForce, ForceMode.Impulse);
        yield return new WaitForSeconds(kbDuration);
        pc.canMove = true;
    }

    private void Death()
    {
        Destroy(gameObject);
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
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Animator anim;

    [SerializeField] float gravity;
    [SerializeField] ParticleSystem dashEffect;
    [SerializeField] GameObject HitboxGameObject;

    [Header("Animation Variables")]
    [SerializeField] GameObject playerDisplay;
    [SerializeField] float playerRotSmoothing;
    Vector3 playerLookDirection;
    float attackAnim;
    float attackTimer;
    float attackCoyoteTime;
    float attackAnimResetTimer;
    
    [Header("Movement")]
    [SerializeField] Transform orientation;
    Vector2 movementInput;
    Vector2 dodgeInput = new Vector2(0f,1f);
    [SerializeField] float lungeForce;
    float currentLungeForce;

    [Header("Movement Variable")]
    [SerializeField] float moveSpeed;
    [SerializeField] float dodgeSpeed; 
    [SerializeField] float dodgeLength;
    bool isDodging = false;
    [SerializeField] float hitKBForce;
    public bool canMove;

    [Header("Cooldowns")]
    [SerializeField] float dodgeCooldownLength;
    float dodgeCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashEffect.Stop();
        playerLookDirection = new Vector3(0,0,1);
        attackAnim = -1;
        canMove = true;
    }

    void Update()
    {
        dodgeCooldown -= Time.deltaTime;

        if (movementInput != Vector2.zero && !isDodging)
        {
            dodgeInput = movementInput;
            playerLookDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        }

        if (!isDodging && canMove)
        {
            rb.linearVelocity = new Vector3(movementInput.x * moveSpeed, rb.linearVelocity.y, movementInput.y * moveSpeed) + new Vector3(playerLookDirection.x * currentLungeForce, 0f, playerLookDirection.z * currentLungeForce);
        }
        else if (canMove && isDodging)
        {
            rb.linearVelocity = new Vector3(playerLookDirection.x * dodgeSpeed, rb.linearVelocity.y, playerLookDirection.z * dodgeSpeed);
        }
        

        
        Quaternion targetRot = Quaternion.LookRotation(playerLookDirection);
        playerDisplay.transform.rotation = Quaternion.Lerp(playerDisplay.transform.rotation, targetRot, playerRotSmoothing * Time.deltaTime);

        //Animator Conditions
        anim.SetBool("Moving", movementInput != Vector2.zero || attackTimer <= 0.2f && attackAnim == 2);
        anim.SetFloat("MoveAnimSpeed", rb.linearVelocity.magnitude/moveSpeed);

        rb.AddForce(gravity * Vector3.down, ForceMode.Force);

        attackTimer += Time.deltaTime;
        attackCoyoteTime -= Time.deltaTime;
        attackAnimResetTimer -= Time.deltaTime;

        if (attackAnimResetTimer <= 0f)
        {
            attackAnim = -1;
        }

        AttackFunc();

        if (attackTimer <= 0.2f && attackAnim == 2)
        {
            currentLungeForce = lungeForce;
        }
        else
        {
            currentLungeForce = 0;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        if (context.performed && dodgeCooldown <= 0f)
        {
            anim.SetTrigger("Dodge");
            StartCoroutine("DodgeDuration");
            dodgeCooldown = dodgeCooldownLength;
            dashEffect.Play();
        }
    }

    private IEnumerator DodgeDuration()
    {
        isDodging = true;
        yield return new WaitForSeconds(dodgeLength);
        isDodging = false;
        dashEffect.Stop();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attackCoyoteTime = 0.1f;
        }
    }

    private void AttackFunc()
    {
        if (attackTimer >= 0.4f && attackCoyoteTime > 0f)
            {
                attackAnim++;
                attackAnimResetTimer = 1f;
                StartCoroutine("AttackHitboxEnable");
                if (attackAnim > 2)
                {
                    attackAnim = 0;
                }

                switch (attackAnim)
                {
                    case 0:
                        anim.Play("playerBirdAttack1");
                        attackTimer = 0;
                        break;
                    case 1:
                        anim.Play("playerBirdAttack2");
                        attackTimer = 0;
                        break;
                    case 2:
                        anim.Play("playerBirdAttack3");
                        attackTimer = 0;
                        break;
                }
            }
    }

    private IEnumerator AttackHitboxEnable()
    {
        HitboxGameObject.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        HitboxGameObject.GetComponent<Collider>().enabled = false;
    }

    public void HitKBFunc()
    {
        rb.AddForce(-playerLookDirection * hitKBForce, ForceMode.Impulse);
    }
}

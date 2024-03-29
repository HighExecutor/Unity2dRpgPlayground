using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float collisoinOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    public int maxHP = 10;
    public int curHP;
    public int level = 1;
    public float xp = 0f;
    public int needXp = 100;
    public float gold = 0;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool canMove = true;
    private bool fireAvailable = true;
    private float fireCooldownCur = 0.0f;
    public float fireCooldown = 5.0f;
    public HealthBar healthBar;
    public XPBar expBar;
    public GoldBar goldBar;
    public GameObject fireballPrefab;
    private bool interaction = false;
    private MerchantInteraction lastMerchant = null;
    private PlayerInput playerInput;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        curHP = maxHP;
        healthBar.SetMaxHealth(maxHP);
        healthBar.SetHealth(maxHP);
        expBar.SetMaxExp(needXp);
        expBar.SetExp(0);
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        if (!fireAvailable)
        {
            if (fireCooldownCur <= 0.0)
            {
                fireCooldownCur = 0.0f;
                fireAvailable = true;
            }
            else
            {
                fireCooldownCur -= Time.fixedDeltaTime;
            }
        }
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);
                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));

                }

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }

                animator.SetBool("IsMoving", success);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return false;
        }

        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            movementSpeed * Time.fixedDeltaTime + collisoinOffset);
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * movementSpeed * Time.fixedDeltaTime);
            return true;
        }

        return false;
    }

    void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("SwordAttack");
        Debug.Log("OnFire");
    }

    void OnFire2()
    {
        if (fireAvailable)
        {
            GameObject fb = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            Fireball fireball = fb.GetComponent<Fireball>();
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            fireball.Init(worldpos, this);
            fireAvailable = false;
            fireCooldownCur = fireCooldown;
        }

    }

    void OnInteract()
    {
        Debug.Log("OnInteract");
        if (lastMerchant != null)
        {
            lastMerchant.OpenDialogue();
            playerInput.SwitchCurrentActionMap("UI");
        }
    }

    public void SwordAttack()
    {
        LockMovement();
        if (spriteRenderer.flipX)
        {
            swordAttack.Attack(false);
        }
        else
        {
            swordAttack.Attack(true);
        }
    }
    
    public void LockMovement()
    {
        canMove = false;
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
    }
    
    public void UnlockMovement()
    {
        canMove = true;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
    
    public int Health
    {
        set
        {
            curHP = value;
            healthBar.SetHealth(curHP);
            if (curHP <= 0)
            {
                // Defeated();
                Debug.Log("Player defeated");
            }
            else
            {
                Debug.Log("Player damage " + value);
                // DamageTrigger();
            }
        }
        get
        {
            return curHP;
        }
    }

    public void GetExp(float exp)
    {
        xp += exp;
        while (xp > needXp)
        {
            level += 1;
            xp -= needXp;
            needXp = 100 * level;
            swordAttack.SetDamage(1f + 0.5f*level);
            maxHP = 10 + 2*level;
            Health = maxHP;
            healthBar.SetMaxHealth(maxHP);
            healthBar.SetHealth(curHP);
            expBar.SetMaxExp(needXp);
            expBar.SetLevel(level);
            
        }
        expBar.SetExp(xp);
    }

    public void TakeGold(float g)
    {
        gold += g;
        goldBar.SetGold(gold);
    }

    public void SetInteraction(bool value, MerchantInteraction merchant)
    {
        interaction = value;
        if (value)
        {
            lastMerchant = merchant;
        }
        else
        {
            lastMerchant = null;
        }
    }

    public void SetInputScheme(string schemeName)
    {
        playerInput.SwitchCurrentActionMap(schemeName);
    }

    void OnSubmit()
        {
            DialogueManager.GetInstance().SetNext(true);
        }

}
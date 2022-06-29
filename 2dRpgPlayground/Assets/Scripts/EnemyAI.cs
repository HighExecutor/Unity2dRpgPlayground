using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 25f;
    public float nextWaypointDistance = 0.1f;
    public float detectRadius = 1f;
    public float attackRadius = 0.2f;

    private Path path;
    private int currentWaypoint = 0;
    private Vector3 startPosition;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Enemy enemy;
    private Transform playerTransform = null;
    private bool canMove = true;
    private bool canAttack = true;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EnemyAttack enemyAttack;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (playerTransform != null)
            {
                float distToPlayer = Vector2.Distance(playerTransform.position, transform.position);
                if (distToPlayer > detectRadius)
                {
                    playerTransform = null;
                    seeker.StartPath(rb.position, startPosition, OnPathComplete);
                }
                else
                {
                    seeker.StartPath(rb.position, playerTransform.position, OnPathComplete);
                }
            }
            else
            {
                if (DetectPlayer())
                {
                    seeker.StartPath(rb.position, playerTransform.position, OnPathComplete);
                }
                else
                {
                    if (Vector2.Distance(rb.position, startPosition) > nextWaypointDistance)
                    {
                        seeker.StartPath(rb.position, startPosition, OnPathComplete);
                    }
                }
            }
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canAttack)
        {
            if (TryAttack())
            {
                return;
            }
        }
        if (canMove)
        {
            TryMove();
        }
    }

    private bool TryAttack()
    {
        if (playerTransform != null)
        {
            float distToPlayer = Vector2.Distance(rb.position, playerTransform.position);
            if (distToPlayer < attackRadius)
            {
                LockMovement();
                animator.SetTrigger("SlimeAttack");
                return true;
            }
        }

        return false;
    }
    
    void Attack()
    {   
        enemyAttack.Attack(!spriteRenderer.flipX);
    }

    void StopAttack()
    {
        UnlockMovement();
        enemyAttack.StopAttack();
    }
    
    public void LockMovement()
    {
        canMove = false;
    }
    
    public void UnlockMovement()
    {
        canMove = true;
    }

    bool TryMove()
    {
        if (path == null)
        {
            animator.SetBool("IsMoving", false);
            return false;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            path = null;
            animator.SetBool("IsMoving", false);
            return false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);
        if (rb.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        } else if (rb.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        animator.SetBool("IsMoving", true);
        return true;
    }

    private bool DetectPlayer()
    {
        Collider2D[] overlapped = Physics2D.OverlapCircleAll(transform.position, detectRadius);
        if (overlapped.Length > 0)
        {
            foreach (var col in overlapped)
            {
                if (col.tag == "Player")
                {
                    playerTransform = col.GetComponent<Transform>();
                    return true;
                }
            }
        }

        return false;
    }
}
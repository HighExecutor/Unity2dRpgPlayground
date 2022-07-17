using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float damage = 1f;
    public float velocity = 1f;
    public float range = 2f;
    public PlayerController player = null;
    public string target = "Enemy";
    public Vector3 direction = Vector3.zero;
    private Animator animator;
    private CircleCollider2D coll;
    private int ticks;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        coll = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        ticks = (int)Mathf.Floor(range / velocity);
    }

    public void Init(Vector3 path, PlayerController link)
    {
        direction = (path - transform.position).normalized;
        player = link;
        transform.eulerAngles = new Vector3(0, 0 ,GetAngleFromVectorFloat(direction));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (coll.enabled)
        {
            if (ticks == 0)
            {
                Explosion();
            }
            else
            {
                ticks--;
                rb.MovePosition(rb.position + (Vector2)direction * velocity * Time.deltaTime);
            }
        }
    }

    void Explosion()
    {
        coll.enabled = false;
        animator.SetTrigger("Explosion");
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(target))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage(damage, player);
            Explosion();
        } else if (col.CompareTag("Obstacle"))
        {
            Explosion();
        }
    }

    private static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }

        return n;
    }
}

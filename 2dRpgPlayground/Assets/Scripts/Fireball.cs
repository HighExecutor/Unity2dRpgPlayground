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
    public Vector2 direction = Vector2.zero;
    private float angle = 0;
    private Rigidbody2D rb;
    private Animator animation;
    private CircleCollider2D collider;
    private int ticks;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
        ticks = (int)Mathf.Floor(range / velocity);
    }

    public void Init(Vector2 path, PlayerController link)
    {
        direction = path;
        player = link;
        angle = Vector2.Angle(direction-(Vector2)transform.position, transform.forward-transform.position);
        Debug.Log("Mouseposition = " + path);
        Debug.Log("Position = " + transform.position);
        Debug.Log("Angle = " + angle);
        rb.MoveRotation(angle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ticks == 0)
        {
            Explosion();
        }
        else
        {
            ticks--;
            Vector2 path = ((Vector2)direction - rb.position).normalized;
            rb.MovePosition(rb.position + path * velocity * Time.deltaTime);
        }
    }

    void Explosion()
    {
        collider.enabled = false;
        animation.SetTrigger("Explosion");
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 1f;
    private Vector2 rightAttackOffset;
    public float xRange;
    public float yRange;

    private void Start()
    {
        rightAttackOffset = transform.localPosition;
    }

    public void Attack(bool right)
    {
        if (right)
        {
            transform.localPosition = rightAttackOffset;
        }
        else
        {
            transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        }

        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position,
            new Vector2(xRange, yRange), 0,
            LayerMask.GetMask("Creatures"));
        Debug.Log("Damaged enemies: " + enemies.Length);
        foreach (var e in enemies)
        {
            if (e.CompareTag("Enemy"))
            {
                Enemy enemy = e.GetComponent<Enemy>();
                enemy.TakeDamage(damage, GetComponentInParent<PlayerController>());
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(xRange, yRange, 1));
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
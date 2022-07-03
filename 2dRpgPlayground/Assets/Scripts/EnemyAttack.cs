using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public int damage = 1;
    private Vector2 rightAttackOffset;
    public float xRange = 0.30f;
    public float yRange = 0.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        rightAttackOffset = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position,
            new Vector2(xRange, yRange), 0,
            LayerMask.GetMask("Creatures"));
        Debug.Log("Damaged enemies: " + collisions.Length);
        foreach (var c in collisions)
        {
            if (c.CompareTag("Player"))
            {
                PlayerController player = c.GetComponent<PlayerController>();
                player.TakeDamage(damage);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(xRange, yRange, 1));
    }

}

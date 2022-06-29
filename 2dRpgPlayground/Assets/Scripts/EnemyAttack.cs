using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public int damage = 1;
    private Vector2 rightAttackOffset;
    private Collider2D attackCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        rightAttackOffset = transform.localPosition;
        attackCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(bool right)
    {
        attackCollider.enabled = true;
        if (right)
        {
            transform.localPosition = rightAttackOffset;
        }
        else
        {
            transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        }
    }

    public void StopAttack()
    {
        attackCollider.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

}

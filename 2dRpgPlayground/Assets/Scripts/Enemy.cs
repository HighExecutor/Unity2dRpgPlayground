using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private float initHealth;
    public float xp = 10f;
    public GameObject goldCoinPrefab;
    
    public float health = 5f;
    public float minGold = 1f;
    public float maxGold = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        initHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(float damage, PlayerController player)
    {
        Debug.Log("Enemy damage " + damage);
        health -= damage;
        if (health <= 0)
        {
            Defeated(player);
        }
        else
        {
            DamageTrigger();
        }
    }
    
   
    public void Defeated(PlayerController killer)
    {
        killer.GetExp(xp);
        animator.SetTrigger("Defeated");
    }

    public void DamageTrigger()
    {
        animator.SetTrigger("Damaged");
    }

    private void RemoveObject()
    {
        GameObject coin = Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
        GoldCoin coinComp = coin.GetComponent<GoldCoin>();
        coinComp.SetGold((int)Random.Range(minGold, maxGold));
        Destroy(gameObject);
    }

    
}

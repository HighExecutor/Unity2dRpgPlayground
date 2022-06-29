using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    public float gold = 1f;

    public void SetGold(float value)
    {
        gold = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeGold(gold);
                Destroy(gameObject);
            }
        }
    }
}

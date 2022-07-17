using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MerchantInteraction : MonoBehaviour
{
    private SpriteRenderer idea;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJson; 

    private PlayerInputActions inputManager;
    // Start is called before the first frame update
    void Start()
    {
        idea = GetComponent<SpriteRenderer>();
        inputManager = new PlayerInputActions();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            idea.enabled = true;
            PlayerController player = other.GetComponent<PlayerController>();
            player.SetInteraction(true, this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        idea.enabled = false;
        PlayerController player = other.GetComponent<PlayerController>();
        player.SetInteraction(false, this);
    }

    public void OpenDialogue()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJson);
    }
}

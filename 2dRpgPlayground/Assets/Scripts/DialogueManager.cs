using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue panel")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")] 
    [SerializeField] private GameObject[] choices;

    private TextMeshProUGUI[] choicesText;
    
    private Story currentStory;
    private bool dialogueIsPlaying;
    
    private static DialogueManager instance;
    private static bool next;
    [SerializeField] private PlayerController player;
    
    private void Awake() {
    if (instance != null) {
        Debug.LogWarning("Found another dialogue manager");
    }
    instance = this;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        next = false;
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void EnterDialogueMode(TextAsset inkJson)
    {
        currentStory = new Story(inkJson.text);
        dialoguePanel.SetActive(true);
        dialogueIsPlaying = true;

        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialoguePanel.SetActive(false);
        dialogueIsPlaying = false;
        dialogueText.text = "";
        player.SetInputScheme("Player");
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (next)
        {
            if (currentStory.canContinue)
            {
                dialogueText.text = currentStory.Continue();
                DisplayChoices();
            }
            else
            {
                StartCoroutine(ExitDialogueMode());
            }

            next = false;
        }
    }

    public void SetNext(bool value)
    {
        next = value;
    }
    public static DialogueManager GetInstance() {
        return instance;
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More chopices than GUI can support");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[index].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }
}

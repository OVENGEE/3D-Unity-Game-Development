using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue Data")]
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitImage;

    [Header("Interaction Event")]
    [SerializeField] private UnityEvent _onInteract;
    UnityEvent IInteractable.onInteract
    {
        get => _onInteract;
        set => _onInteract = value;
    }

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;

    public bool CanInteract()
    {
        return !isDialogueActive && dialogueData != null;
    }

    public void Interact()
    {
        if (dialogueData == null) return;

        // If dialogue not active -> start it
        if (!isDialogueActive)
        {
            StartDialogue();
            _onInteract?.Invoke();
            return;
        }

        // If currently typing, finish the line instantly
        if (isTyping)
        {
            StopAllCoroutines();
            if (dialogueIndex >= 0 && dialogueIndex < dialogueData.dialogueLines.Length)
                dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            return;
        }

        // Otherwise go to next line or end
        NextLine();
    }

    void StartDialogue()
    {
        //Cursor visibility
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (dialogueData == null || dialogueData.dialogueLines.Length == 0) return;

        isDialogueActive = true;
        dialogueIndex = 0;

        if (nameText != null) nameText.SetText(dialogueData.npcName);
        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            if (dialogueIndex >= 0 && dialogueIndex < dialogueData.dialogueLines.Length)
                dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            return;
        }

        dialogueIndex++;
        if (dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        if (dialogueText != null) dialogueText.SetText("");

        string line = dialogueData.dialogueLines[dialogueIndex];
        foreach (char letter in line)
        {
            if (dialogueText != null) dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines != null &&
            dialogueData.autoProgressLines.Length > dialogueIndex &&
            dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        if (dialogueText != null) dialogueText.SetText("");
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        //Cursor lock and make invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

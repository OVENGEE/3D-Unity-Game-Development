using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName ="NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;// Name of the NPC

    public string[] dialogueLines; // Array of dailouge lines} 
    public bool[] autoProgressLines;// Array of booleans for auto-progressing lines

    public float autoProgressDelay = 1.5f; // Array of booleans for auto-progressing lines with delay

    public float typingSpeed = 0.05f; // Speed of typing effect

    public AudioClip voiceSound;

    public float voicePitch = 1f;

    
}

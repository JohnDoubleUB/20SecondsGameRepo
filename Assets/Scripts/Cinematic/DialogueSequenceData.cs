using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "DialogueSequenceData", menuName = "Scriptable Objects/DialogueSequenceData")]
public class DialogueSequenceData : ScriptableObject
{
    public DialogueSection[] DialogueSections;
}

[System.Serializable]
public class DialogueLine 
{
    public string Line;
    public AudioResource Audio;
    public float EndDelay = 1f;
}

[System.Serializable]
public class DialogueSection 
{
    public float TimePerWord = 0.5f;
    public bool LoopAudioResources = true;
    public AudioResource[] AudioResources;
    public DialogueLine[] DialogueLines;
}

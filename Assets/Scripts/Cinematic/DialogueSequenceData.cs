using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "DialogueSequenceData", menuName = "Scriptable Objects/DialogueSequenceData")]
public class DialogueSequenceData : ScriptableObject
{
    public bool skippable = false;
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
public class CustomAudioResource 
{
    public AudioResource Resource;
    public float Pitch = 1f;
}

[System.Serializable]
public class DialogueSection 
{
    public bool StopPreviousSectionAudio = false;
    public float TimePerWord = 0.5f;
    public bool LoopAudioResources = true;
    public CustomAudioResource[] AudioResources;
    public DialogueLine[] DialogueLines;
}

using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "DialogueSequenceData", menuName = "Scriptable Objects/DialogueSequenceData")]
public class DialogueSequenceData : ScriptableObject
{
    public string SequenceName; //Needed if skippable if seen before is checked
    public bool skippableIfSeenBefore = false;
    public bool skippable = false;
    public DialogueSection[] DialogueSections;

    public bool IsSkippable()
    {
        if(skippable) 
        {
            return true;
        }

        if(skippableIfSeenBefore && SequenceName.Length > 0) 
        {
            return PlayerPrefs.GetInt(SequenceName, 0) == 1;
        }

        return false;
    }

    public void SaveAsSeen() 
    {
        if (skippableIfSeenBefore && SequenceName.Length > 0) 
        {
            PlayerPrefs.SetInt(SequenceName, 1);
            PlayerPrefs.Save(); //Called here because normally would be called OnApplicationQuit however web gl doesn't ever call this method
        }
    }
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

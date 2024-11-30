using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UIManagerLibrary.Scripts;
using UnityEngine;
using UnityEngine.Audio;

public class DialogueSequencePlayer : MonoBehaviour
{
    public static DialogueSequencePlayer current;

    public TextMeshProUGUI DialogueText;

    public DialogueSequenceData Sequence { get; private set; }
    private Action OnFinishCallback = null;

    private int DialogueSectionIndex = -1;
    private int DialogueLineIndex = -1;

    private float DialogueTimer = 0;
    
    [SerializeField]
    private AudioSource DialogueAudioSource;

    private List<AudioSource> BackgroundAudioSources = new List<AudioSource>();
    
    [SerializeField]
    private AudioMixerGroup BackgroundMixerGroup;

    [SerializeField]
    private GameObject BackgroundAudioObject;

    public void StartDialogueSequence(DialogueSequenceData sequence, Action onFinishCallback = null) 
    {
        UIManager.current.SetActiveContexts(true, true, "Cinematic");
        OnFinishCallback = onFinishCallback;
        Sequence = sequence;
        DialogueSectionIndex = -1;
        DialogueLineIndex = -1;
    }

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

    private void Start()
    {
        PlayerController.current.OnSkipButtonPressed += OnSkip;
    }

    private void OnSkip()
    {
        if (Sequence == null || !Sequence.skippable) 
        {
            return;
        }

        DialogueAudioSource.Stop();
        DialogueTimer = 0;
    }

    private bool SetDialogueText(string text) 
    {
        if (DialogueText == null) 
        {
            return false;
        }

        DialogueText.text = text;

        return true;
    }

    private void DestroyAllBackgroundAudioSources() 
    {
        foreach (AudioSource source in BackgroundAudioSources)
        {
            source.Stop();
            source.resource = null;
            Destroy(source);
        }

        BackgroundAudioSources.Clear();
    }

    private void SetDialogueSectionAudio(bool looping, AudioResource[] audioResources) 
    {
        int audioResourceLength = audioResources.Length;
        int currentAudioSourceLength = BackgroundAudioSources.Count;

        if (currentAudioSourceLength > 0) 
        {
            foreach (AudioSource source in BackgroundAudioSources) 
            {
                source.loop = false;
                //source.Stop();
                //source.resource = null;
            }
        }

        for (int i = 0; i < audioResourceLength; i++) 
        {
            if (BackgroundAudioSources.Count < i + 1)
            {
                AudioSource newAudioSource = InstantiateBackgroundAudioSource();
                newAudioSource.resource = audioResources[i];
                newAudioSource.loop = looping;
                newAudioSource.Play();
                BackgroundAudioSources.Add(newAudioSource);
            }
            else 
            {
                BackgroundAudioSources[i].resource = audioResources[i];
                BackgroundAudioSources[i].loop = looping;
                BackgroundAudioSources[i].Play();
            }
        }
    }

    private AudioSource InstantiateBackgroundAudioSource() 
    {
        AudioSource newAudioSource = BackgroundAudioObject.AddComponent<AudioSource>();
        newAudioSource.outputAudioMixerGroup = BackgroundMixerGroup;
        newAudioSource.playOnAwake = false;
        return newAudioSource;
    }

    private bool SetDialogueLineAudio(AudioResource dialogueAudio) 
    {
        //No implementation yet
        if (DialogueAudioSource != null && dialogueAudio) 
        {
            DialogueAudioSource.resource = dialogueAudio;
            DialogueAudioSource.Play();
            return true;
        }

        return false;
    }

    private bool ShouldContinueToNextDialogueSection() 
    {
        if (DialogueSectionIndex == -1) 
        {
            return true;
        }

        if (DialogueLineIndex < Sequence.DialogueSections[DialogueSectionIndex].DialogueLines.Length) 
        {
            return false;
        }

        return true;
    }



    private bool TryIncrementDialogueSection() 
    {
        int newDialogueSectionIndex = DialogueSectionIndex + 1;
        if(newDialogueSectionIndex < Sequence.DialogueSections.Length) 
        {
            if (DialogueSectionIndex > -1) 
            {

            }


            DialogueSectionIndex = newDialogueSectionIndex;
            return true;
        }

        return false;
    }

    private void SetDialogue(DialogueLine dialogue, DialogueSection section) 
    {
        
        SetDialogueText(dialogue.Line);

        Debug.Log("Set Dialogue: " + dialogue.Line);

        if (SetDialogueLineAudio(dialogue.Audio)) 
        {
            DialogueTimer = dialogue.EndDelay;
            Debug.Log("Dialogue Time: " + DialogueTimer);
            return;
        }

        DialogueTimer = (dialogue.Line.Split(' ').Length * section.TimePerWord) + dialogue.EndDelay;

        Debug.Log("Dialogue Time: " + DialogueTimer);

    }

    private void OnSequenceEnd() 
    {
        DestroyAllBackgroundAudioSources();

        Sequence = null;

        if (OnFinishCallback != null)
        {
            OnFinishCallback();
        }

        OnFinishCallback = null;

        UIManager.current.SetActiveContexts(false, true, "Cinematic");
    }

    private void Update()
    {
        if(Sequence == null) 
        {
            return;
        }

        if (DialogueAudioSource.isPlaying)
        {
            return;
        }

        if (DialogueTimer > 0) 
        {
            DialogueTimer -= Time.deltaTime;
            return;
        }


        if(ShouldContinueToNextDialogueSection()) //If starting a new dialogue section
        {
            if (!TryIncrementDialogueSection()) 
            {
                //If we get here then its the end of the sequence
                OnSequenceEnd();
                return;
            }

            DialogueSection newSection = Sequence.DialogueSections[DialogueSectionIndex];
            SetDialogueSectionAudio(newSection.LoopAudioResources, newSection.AudioResources);

            //STryIncrementDialogueSectionetup new looping audio (if any)
            DialogueLineIndex = 1;

            DialogueLine newDialogue = newSection.DialogueLines[0];

            SetDialogue(newDialogue, newSection);

            return;
        }

        

        DialogueSection currentSection = Sequence.DialogueSections[DialogueSectionIndex];
        DialogueLine nextDialogue = currentSection.DialogueLines[DialogueLineIndex];

        SetDialogue(nextDialogue, currentSection);

        DialogueLineIndex++;
    }
}

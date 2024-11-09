using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioClipObject : MonoBehaviour
{
    public AudioSource Source;

    private void Reset()
    {
        Source = GetComponent<AudioSource>();
    }

}
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;
    public float SoundVolumeMultiplier = 1;

    public float pitchVariation = 0.2f;

    [SerializeField]
    private AudioClipObject audioClipPrefab;

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

    private float GenerateRandomPitch()
    {
        return Random.Range(1f - pitchVariation, 1f + pitchVariation);
    }

    public AudioClipObject PlayClipAt(AudioClip clip, Vector3 pos, float volume, bool withPitchVariation = true, float delayInSeconds = 0f)
    {
        return PlayClipAt(clip, pos, volume, withPitchVariation ? GenerateRandomPitch() : 1, delayInSeconds);
    }

    public AudioClipObject PlayClipAt(AudioClip clip, Vector3 pos, float volume, float pitch, float delayInSeconds)
    {
        AudioClipObject newTempObject = CreateNewAudioClipObject(pos);
        newTempObject.Source.spatialBlend = 0.5f;
        newTempObject.Source.clip = clip; // define the clip
        newTempObject.Source.pitch = pitch;
        newTempObject.Source.volume = volume * SoundVolumeMultiplier;

        newTempObject.Source.PlayDelayed(delayInSeconds); // start the sound
        Destroy(newTempObject.gameObject, clip.length + delayInSeconds); // destroy object after clip duration

        return newTempObject; // return the AudioSource reference
    }

    public AudioClipObject PlayClipAt(AudioClip clip, Transform transform, float volume, bool withPitchVariation = true, bool shouldBeParented = false, float delayInSeconds = 0f)
    {
        return PlayClipAt(clip, transform, shouldBeParented, volume, withPitchVariation ? GenerateRandomPitch() : 1, delayInSeconds);
    }

    public AudioClipObject PlayClipAt(AudioClip clip, Transform transform, bool shouldBeParented, float volume, float pitch, float delayInSeconds)
    {
        AudioClipObject newTempObject = shouldBeParented ? CreateNewAudioClipObject(transform) : CreateNewAudioClipObject(transform.position);
        newTempObject.Source.spatialBlend = 0.5f;
        newTempObject.Source.clip = clip; // define the clip
        newTempObject.Source.pitch = pitch;
        newTempObject.Source.volume = volume * SoundVolumeMultiplier;

        newTempObject.Source.PlayDelayed(delayInSeconds); // start the sound
        Destroy(newTempObject.gameObject, clip.length + delayInSeconds); // destroy object after clip duration

        return newTempObject; // return the AudioSource reference
    }

    private AudioClipObject CreateNewAudioClipObject(Vector3 pos)
    {
        //If we have a default prefab, use this always
        if (audioClipPrefab)
        {
            return Instantiate(audioClipPrefab, pos, Quaternion.identity);
        }

        //Create gameobject
        GameObject newObject = new GameObject("TempAudio");
        newObject.transform.position = pos;

        return newObject.AddComponent<AudioClipObject>();
    }

    private AudioClipObject CreateNewAudioClipObject(Transform transform)
    {
        //If we have a default prefab, use this always
        if (audioClipPrefab)
        {
            return Instantiate(audioClipPrefab, transform);
        }

        //Create gameobject
        GameObject newObject = new GameObject("TempAudio");


        newObject.transform.parent = transform;
        newObject.transform.position = Vector3.zero;


        return newObject.AddComponent<AudioClipObject>();
    }
}

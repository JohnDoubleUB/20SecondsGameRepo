using UnityEngine;
using UnityEngine.Audio;

public class PickupItem : MonoBehaviour
{
    public string Name;
    public int Index;
    public Sprite Icon;
    public AudioClip Audio;
    public float AudioVolume = 0.2f;

    public PickedUpItem GetPickedUpItem() 
    {
        return new PickedUpItem { Index = Index, Icon = Icon, Name = Name };
    }

    public PickedUpItem Pickup() 
    {
        PickedUpItem item = GetPickedUpItem();
        if(AudioManager.current) 
        {
            AudioManager.current.PlayClipAt(Audio, transform.position, AudioVolume);
        }
        Destroy(gameObject);
        return item;
    }
}

[System.Serializable]
public class PickedUpItem 
{
    public int Index;
    public Sprite Icon;
    public string Name;
}

using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string Name;
    public int Index;
    public Sprite Icon;

    public PickedUpItem GetPickedUpItem() 
    {
        return new PickedUpItem { Index = Index, Icon = Icon, Name = Name };
    }

    public PickedUpItem Pickup() 
    {
        PickedUpItem item = GetPickedUpItem();
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

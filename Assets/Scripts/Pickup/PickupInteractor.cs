using UnityEngine;

public class PickupInteractor : MonoBehaviour
{
    public delegate void InteractingItemUpdate(string interactingItemName);
    public event InteractingItemUpdate OnInteractingItemUpdate;

    [SerializeField]
    private Camera Camera;

    public float MaxRayDistance = 200f;

    public LayerMask LayerMask;

    public bool Enabled { get; private set; } = true;

    public PickupItem CurrentPickupItem = null;
    public GameObject CurrentPickupGameObject = null;

    public void EnablePickupInteraction(bool enabled) 
    {
        Enabled = enabled;
        //Ensure that the item text is cleared while interaction is not enabled
        OnInteractingItemUpdate?.Invoke(Enabled && CurrentPickupItem != null ? CurrentPickupItem.Name : "");
    }

    //Perform the pickup!
    public bool TryGetPickup(out PickedUpItem pickedUpItem) 
    {
        if (CurrentPickupItem == null) 
        {
            pickedUpItem = null;
            return false;
        }

        pickedUpItem = CurrentPickupItem.Pickup();

        CurrentPickupItem = null;
        CurrentPickupGameObject = null;

        return true;
    }

    private void Update()
    {
        if(!Enabled) 
        {
            return;
        }

        if (Camera == null) 
        {
            return;
        }

        PickupItemUpdate();
    }

    //Returns true if the item has changed
    private void PickupItemUpdate() 
    {
        Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, MaxRayDistance, LayerMask))
        {
            //Get the object we hit
            GameObject hitObject = hit.collider.gameObject;

            //Check if we currently have this gameobject
            if (CurrentPickupGameObject != null && CurrentPickupGameObject == hitObject)
            {
                return;
            }

            //Store the gameobject, and also try and get the pickup item
            CurrentPickupGameObject = hitObject;
            CurrentPickupItem = hitObject.GetComponent<PickupItem>();
            OnInteractingItemUpdate?.Invoke(CurrentPickupItem != null ? CurrentPickupItem.Name : "");
        }
        else if(CurrentPickupItem != null) 
        {
            CurrentPickupItem = null;
            CurrentPickupGameObject = null;
            OnInteractingItemUpdate?.Invoke("");
        }


    }
}

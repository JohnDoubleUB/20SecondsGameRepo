using UnityEngine;

public class MouseInteractor : MonoBehaviour
{
    public float MaxRayDistance = 1000f;

    public LayerMask LayerMask;

    public MouseInteractable HeldInteractable;

    [SerializeField]
    private Camera Camera;

    public bool ButtonHeld { get; private set; }

    public void SetButton(bool value)
    {
        ButtonHeld = value;

        if (value) 
        {
            TryAndHitObject();
        }
        else
        {
            if(HeldInteractable != null) 
            {
                HeldInteractable.Interact(false);
            }
        }


    }

    public void TryAndHitObject()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, MaxRayDistance, LayerMask)) 
        {
            GameObject hitObj = hit.collider.gameObject;

            if (HeldInteractable != null && hitObj == HeldInteractable.gameObject)
            {
                HeldInteractable.Interact(true);
            }

            MouseInteractable newInteractable = hitObj.GetComponent<MouseInteractable>();

            if (HeldInteractable != null) 
            {
                HeldInteractable.Interact(false);
            }

            if (newInteractable != null) 
            {
                newInteractable.Interact(true);
            }

            HeldInteractable = newInteractable;

            return;
        }

        if (HeldInteractable != null)
        {
            HeldInteractable.Interact(false);
        }

        HeldInteractable = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (ButtonHeld && HeldInteractable != null && HeldInteractable.Held)
        {
            TryAndHitObject();
        }
    }

}

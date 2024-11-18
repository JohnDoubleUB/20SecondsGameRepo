using System;
using UnityEngine;

public class MouseInteractablePanel : MouseInteractable
{
    [SerializeField]
    private AudioSource AudioSource;

    public float MaxRotation = 180;

    Vector3 MousePositionOrigin = Vector3.zero;
    float range = 500;
    float Max = 0;
    float Min = 0;

    public float mouseXLastFrame;
    public float rotationYDifference;

    private float rotationYLastFrame;
    private float initialYRotation;
    private float rotationChange;
    private float rotationVelocity;

    private void Reset()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        ControlUntilRelease = true;
    }

    protected override void OnInteract(bool value)
    {
        if (value)
        {
            MousePositionOrigin = Input.mousePosition;
            Max = MousePositionOrigin.x + range;
            Min = MousePositionOrigin.x - range;
            initialYRotation = transform.localRotation.eulerAngles.y;
        }
    }

    private void PanelUnheldUpdate()
    {
        if (rotationYDifference < 0)
        {
            rotationYDifference = Mathf.Min(rotationYDifference + Time.deltaTime * 10, 0);
        }
        else if (rotationYDifference > 0)
        {
            rotationYDifference = Mathf.Max(rotationYDifference - Time.deltaTime * 10, 0);
        }
        else
        {
            AudioSource.Stop();
            return;
        }

        Vector3 rotation = transform.localRotation.eulerAngles;

        rotation.y = Mathf.Clamp(rotation.y + rotationYDifference, 0, MaxRotation);
        transform.localRotation = Quaternion.Euler(rotation);

        if (rotation.y == 180 || rotation.y == 0)
        {
            rotationYDifference = 0;
        }
    }

    private void PanelHeldUpdate()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;

        Vector3 currentMousePos = Input.mousePosition;


        if (mouseXLastFrame > currentMousePos.x || mouseXLastFrame < currentMousePos.x)
        {
            rotationYDifference = (mouseXLastFrame - currentMousePos.x) * 0.3f;
        }
        else
        {
            rotationYDifference = 0;
        }

        // Clamp the mouse position within this min and max range
        float positionClamped = Mathf.Clamp(currentMousePos.x, Min, Max);

        // Remap the clamped position to a range from 1 to 0
        float remappedRange = positionClamped.Remap(Min, Max, 1, 0);

        // Get the current rotation angles
        rotation.y = Mathf.Clamp(initialYRotation + remappedRange.Remap(0, 1, -MaxRotation, MaxRotation), 0, MaxRotation);
        transform.localRotation = Quaternion.Euler(rotation);

        rotationChange = Mathf.SmoothDamp(rotationChange, rotation.y - rotationYLastFrame, ref rotationVelocity, 0.02f);

        //if (rotated != rotationChange > 0) 
        //{
        //    rotated = !rotated;
        //    AudioSource.Play();
            
        //}

        if (Mathf.Abs(rotationChange) < 0.0001f) 
        {
            AudioSource.Stop();
        }
        else 
        {
            if (!AudioSource.isPlaying) 
            {
                AudioSource.Play();
            }
        }

        rotationYLastFrame = rotation.y;
        mouseXLastFrame = currentMousePos.x;
    }

    private void Update()
    {
        if (!Held)
        {
            PanelUnheldUpdate();
            return;
        }

        PanelHeldUpdate();
    }
}

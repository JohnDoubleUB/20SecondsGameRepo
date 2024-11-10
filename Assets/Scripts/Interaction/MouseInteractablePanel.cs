using System;
using UnityEngine;

public class MouseInteractablePanel : MouseInteractable
{
    Vector3 MousePositionOrigin = Vector3.zero;
    float range = 500;
    float Max = 0;
    float Min = 0;

    public float mouseXLastFrame;
    public float rotationYDifference;

    private float initialYRotation;

    private float yVelocity = 0f;

    //https://discussions.unity.com/t/how-i-can-access-objects-hinge-joint-and-change-its-value-with-c/819080/2

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
            initialYRotation = transform.rotation.eulerAngles.y;
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
            return;
        }

        Vector3 rotation = transform.rotation.eulerAngles;

        rotation.y = Mathf.Clamp(rotation.y + rotationYDifference, 0, 180);
        transform.rotation = Quaternion.Euler(rotation);

        if (rotation.y == 180 || rotation.y == 0)
        {
            rotationYDifference = 0;
        }
    }

    private void PanelHeldUpdate()
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        Vector3 currentMousePos = Input.mousePosition;


        if (mouseXLastFrame > currentMousePos.x || mouseXLastFrame < currentMousePos.x)
        {
            rotationYDifference = (mouseXLastFrame - currentMousePos.x) * 0.3f;
        }
        else
        {
            rotationYDifference = 0;
            //if (rotationYDifference < 0)
            //{
            //    rotationYDifference = Mathf.Min(rotationYDifference + Time.deltaTime * 10, 0);
            //}
            //else if (rotationYDifference > 0)
            //{
            //    rotationYDifference = Mathf.Max(rotationYDifference - Time.deltaTime * 10, 0);
            //}
            //else { }
        }



        // Define the min and max based on the current mouse position with an offset

        // Clamp the mouse position within this min and max range
        float positionClamped = Mathf.Clamp(currentMousePos.x, Min, Max);

        // Remap the clamped position to a range from 1 to 0
        float remappedRange = positionClamped.Remap(Min, Max, 1, 0);

        // Get the current rotation angles


        rotation.y = Mathf.Clamp(initialYRotation + remappedRange.Remap(0, 1, -180, 180), 0, 180);
        transform.rotation = Quaternion.Euler(rotation);

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

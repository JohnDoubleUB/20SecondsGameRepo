using UnityEngine;

public class MouseInteractablePanel : MouseInteractable
{
    Vector3 MousePositionOrigin = Vector3.zero;
    float range = 5;
    float Max = 0;
    float Min = 0;

    //https://discussions.unity.com/t/how-i-can-access-objects-hinge-joint-and-change-its-value-with-c/819080/2

    protected override void OnInteract(bool value)
    {
        Debug.Log("Interacted!!");
        if(value)
        {
            MousePositionOrigin = Input.mousePosition;
            Max = MousePositionOrigin.x + range;
            Min = MousePositionOrigin.x - range;
        }
    }

    private void FixedUpdate()
    {
        if(!Held)
        {
            return;
        }

        Vector3 currentMousPos = Input.mousePosition;

        float remappedRange = Mathf.Clamp(currentMousPos.x, Min, Max).Remap(Min, Max, 0, 1);

        Debug.Log("Range " + remappedRange);

    }
}

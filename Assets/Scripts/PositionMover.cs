using UnityEngine;

public class PositionMover : MonoBehaviour
{
    public Transform TargetTransform;

    private void FixedUpdate()
    {
        if(TargetTransform == null) 
        {
            return;
        }

        transform.position = TargetTransform.position;
        transform.rotation = TargetTransform.rotation;
    }
}

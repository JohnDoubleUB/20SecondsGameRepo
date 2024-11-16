using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class ConnectionPoint 
{
    public int IntendedId;
    public MouseInteractableWire Wire;
    public Transform Point;
    public float LastDistance;
}

public class WireConnectionPoints : MonoBehaviour
{
    [SerializeField]
    private ConnectionPoint[] ConnectionPoints;

    [SerializeField]
    private float MaxValidDistance = 2f;

    public bool TryConnectToNearestPoint(Vector3 position, MouseInteractableWire wire, out ConnectionPoint point, out int index)
    {
        point = null;
        index = -1;

        for (int i = 0; i < ConnectionPoints.Length; i++)
        {
            ConnectionPoint connectionPoint = ConnectionPoints[i];

            if (connectionPoint.Wire != null)
            {
                continue;
            }

            connectionPoint.LastDistance = Vector3.Distance(position, connectionPoint.Point.position);

            if (connectionPoint.LastDistance > MaxValidDistance)
            {
                continue;
            }

            if (point == null || connectionPoint.LastDistance < point.LastDistance)
            {
                point = connectionPoint;
                index = i;
            }
        }

        if(point != null) 
        {
            point.Wire = wire;
            return true;
        }

        return false;
    }

    public void ClearIndex(int index) 
    {
        ConnectionPoints[index].Wire = null;
    }
}

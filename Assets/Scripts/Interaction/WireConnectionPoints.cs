using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
    AudioSource Audio;

    [SerializeField]
    AudioResource PlugSound;

    [SerializeField]
    AudioResource UnplugSound;

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
            PlayAudio(PlugSound);

            return true;
        }

        PlayAudio(UnplugSound);
        return false;
    }

    public void PlayAudio(AudioResource sound) 
    {
        if (Audio == null) 
        {
            return;
        }

        if(sound == null) 
        {
            return; 
        }

        Audio.resource = sound;
        Audio.Play();
    }

    public void ClearIndex(int index) 
    {
        ConnectionPoints[index].Wire = null;
        PlayAudio(UnplugSound);
    }
}

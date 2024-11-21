using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

[System.Serializable]
public class ConnectionPoint 
{
    public MouseInteractableWire Wire;
    public Transform Point;
    public MeshRenderer Mesh;
    public float LastDistance;
}

public class WireConnectionPoints : MonoBehaviour
{
    public delegate void AllWiresConnected(int[] wireIndexes);
    public event AllWiresConnected OnAllWiresConnected;

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

    public void SetMaterialForPoints(Material[] materials) 
    {
        for (int i = 0; i < materials.Length && i < ConnectionPoints.Length; i++) 
        {
            ConnectionPoint p = ConnectionPoints[i];
            
            if (p.Mesh == null) 
            {
                continue;
            }

            p.Mesh.material = materials[i];
        }

    }

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

            CheckAndReportIfAllConnected();
            return true;
        }

        PlayAudio(UnplugSound);
        return false;
    }

    private void CheckAndReportIfAllConnected() 
    {
        int[] wireIndexes = new int[ConnectionPoints.Length];

        for (int i = 0; i < ConnectionPoints.Length; i++) 
        {
            MouseInteractableWire wire = ConnectionPoints[i].Wire;
            if (wire == null) 
            {
                return;
            }

            wireIndexes[i] = wire.Index;
        }

        OnAllWiresConnected?.Invoke(wireIndexes);
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

    public void ClearIndex(int index, bool playSound = true) 
    {
        ConnectionPoints[index].Wire = null;
        
        if (playSound) 
        {
            PlayAudio(UnplugSound); 
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class MouseInteractableWire : MouseInteractable
{
    public int Index;

    [SerializeField]
    private MeshRenderer ColouredBodyMesh;

    [SerializeField]
    private Transform StartAnchor;

    [SerializeField]
    private Rigidbody EndAnchor;

    [SerializeField]
    private Transform MaxPullDownAnchor;

    [SerializeField]
    private Transform WireParent;

    [SerializeField]
    private WireController Wire;

    [SerializeField]
    private float CloserToCameraOffset = 0.01f;

    Vector3 AnchorStartInitialPositionLocal;

    Vector3 AnchorStartOnGrabPositionLocal;

    private float DistanceFromCamera;

    private Vector3 InitialMousePositionInWorld;

    private Vector3 AnchorEndInitialPositionLocal;
    private Quaternion AnchorEndInitialRotationLocal;

    [SerializeField]
    private WireConnectionPoints ConnectionPoints;

    private int ConnectedIndex = -1;

    private Vector3 lastPositionInWorld = Vector3.zero;

    private void Start()
    {
        AnchorStartInitialPositionLocal = StartAnchor.localPosition;
        AnchorEndInitialPositionLocal = EndAnchor.transform.localPosition;
        AnchorEndInitialRotationLocal = EndAnchor.transform.localRotation;
    }

    public Material GetColouredMaterial() 
    {
        return ColouredBodyMesh.material;
    } 

    protected override void OnInteract(bool value, bool triggeredByReset = false)
    {
        if (value)
        {
            DistanceFromCamera = Vector3.Distance(WireParent.transform.position, Camera.main.transform.position) - CloserToCameraOffset;
            InitialMousePositionInWorld = GetMousePositionInWorld();
            AnchorStartOnGrabPositionLocal = StartAnchor.localPosition;
            EndAnchor.isKinematic = false;

            BreakConnection();

        }
        else 
        {
            CheckForConnectionOrReset(triggeredByReset);
        }
    }

    private void CheckForConnectionOrReset(bool triggeredByReset) 
    {
        if(ConnectionPoints != null && ConnectionPoints.TryConnectToNearestPoint(lastPositionInWorld, this, out ConnectionPoint point, out int index, !triggeredByReset)) 
        {
            EndAnchor.isKinematic = true;
            ConnectedIndex = index;
            EndAnchor.transform.SetPositionAndRotation(point.Point.transform.position, point.Point.transform.rotation);
            StartAnchor.localPosition = new Vector3(StartAnchor.localPosition.x, MaxPullDownAnchor.localPosition.y, StartAnchor.localPosition.z);

            return;
        }

        ResetCable();
    }

    protected override void OnResetInteractable()
    {
        BreakConnection(false);
        ResetCable();
    }

    public void ResetIfNotConnnected()
    {
        if (ConnectedIndex != -1 && ConnectionPoints != null)
        {
            return;
        }

        Debug.Log("Reset because not connected!");

        ResetCable();
    }

    private void BreakConnection(bool playSound = true) 
    {
        if (ConnectedIndex != -1 && ConnectionPoints != null)
        {
            ConnectionPoints.ClearIndex(ConnectedIndex, playSound);
            ConnectedIndex = -1;
        }
    }

    private void ResetCable() 
    {
        ConnectedIndex = -1;
        Wire.ResetSegmentsToCachedPositions();
        EndAnchor.transform.localPosition = AnchorEndInitialPositionLocal;
        EndAnchor.transform.localRotation = AnchorEndInitialRotationLocal;
    }

    private void Update()
    {
        if (!Held)
        {
            if (ConnectedIndex == -1)
            {
                StartAnchor.localPosition = AnchorStartInitialPositionLocal;
            }
            return;
        }

        WireHeldUpdate();
    }

    private Vector3 GetMousePositionInWorld() 
    {
        Vector3 v3 = Input.mousePosition;
        v3.z = DistanceFromCamera;
        v3 = Camera.main.ScreenToWorldPoint(v3);

        return v3;
    }

    private void WireHeldUpdate()
    {
        Vector3 mousePositionInWorld = GetMousePositionInWorld();
        Vector3 offset = InitialMousePositionInWorld - mousePositionInWorld;

        StartAnchor.localPosition = new Vector3(AnchorStartInitialPositionLocal.x, Mathf.Clamp(AnchorStartOnGrabPositionLocal.y - offset.y, MaxPullDownAnchor.localPosition.y, AnchorStartInitialPositionLocal.y), AnchorStartInitialPositionLocal.z);

        EndAnchor.position = mousePositionInWorld;

        lastPositionInWorld = mousePositionInWorld;
    }

}

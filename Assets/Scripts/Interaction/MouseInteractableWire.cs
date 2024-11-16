using UnityEngine;

public class MouseInteractableWire : MouseInteractable
{
    [SerializeField]
    private Transform StartAnchor;

    [SerializeField]
    private Transform EndAnchor;

    [SerializeField]
    private Transform MaxPullDownAnchor;

    [SerializeField]
    private Transform TestObject;

    [SerializeField]
    private Transform WireParent;

    [SerializeField]
    private WireController Wire;

    //[SerializeField]
    private float CloserToCameraOffset = 0.01f;

    Vector2 MousePositionOrigin = Vector2.zero;

    Vector3 AnchorStartInitialPositionLocal;

    Vector3 AnchorStartOnGrabPositionLocal;

    private float DistanceFromCamera;

    private Vector3 InitialMousePositionInWorld;



    private void Start()
    {
        AnchorStartInitialPositionLocal = StartAnchor.localPosition;
    }

    protected override void OnInteract(bool value)
    {
        if (value)
        {
            //MousePositionOrigin = Camera.main.WorldToScreenPoint(transform.position);
            MousePositionOrigin = Input.mousePosition;
            DistanceFromCamera = Vector3.Distance(WireParent.transform.position, Camera.main.transform.position) - CloserToCameraOffset;
            InitialMousePositionInWorld = GetMousePositionInWorld();
            AnchorStartOnGrabPositionLocal = StartAnchor.localPosition;
        }
        else 
        {
            Wire.ResetSegmentsToCachedPositions();
        }
    }

    private void Update()
    {
        if (!Held)
        {
            StartAnchor.localPosition = AnchorStartInitialPositionLocal;
            return;
        }

        WireHeldUpdate();
    }

    private void WireUnHeldUpdate() 
    {

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

        //Vector2 offset = new Vector2((MousePositionOrigin.x - currentMousePos.x + 0.2f) * 0.01f, (MousePositionOrigin.y - currentMousePos.y + 0.2f) * 0.01f);

        //Vector3 lookPosition = (WireParent.transform.forward * offset.x) + (-WireParent.transform.up * offset.y);

        TestObject.position = mousePositionInWorld;

        EndAnchor.position = mousePositionInWorld;

        //SetLookAtX(dialObject.transform, dialObject.transform.position + lookPosition, MinMaxCumulativeRotation);


    }

}

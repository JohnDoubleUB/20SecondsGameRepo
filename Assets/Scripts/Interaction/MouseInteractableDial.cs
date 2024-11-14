using UnityEngine;

public class MouseInteractableDial : MouseInteractable
{
    Vector2 MousePositionOrigin = Vector2.zero;
    float range = 500;

    [SerializeField]
    GameObject dialObject;


    public Vector2 MinMaxCumulativeRotation = new Vector2(-3600, 3600);
    public float cumulativeRotationX = 0f;  // Track cumulative X-axis rotation

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ControlUntilRelease = true;
    }

    private void Start()
    {
    }

    protected override void OnInteract(bool value)
    {
        if (value)
        {
            MousePositionOrigin = Input.mousePosition;
        }
    }

    void SetLookAtX(Transform target, Vector3 lookAtPosition)
    {
        Vector3 direction = lookAtPosition - target.position;

        float angleX = Vector3.SignedAngle(transform.forward, direction, transform.right) + 90f;

        target.localRotation = Quaternion.AngleAxis(angleX, Vector3.right);
    }

    void SetLookAtX(Transform target, Vector3 lookAtPosition, Vector2 MinMaxClamp)
    {
        Vector3 direction = lookAtPosition - target.position;

        float angleX = Vector3.SignedAngle(target.forward, direction, target.right) + 90f;

        cumulativeRotationX = Mathf.Clamp(cumulativeRotationX + angleX, MinMaxClamp.x, MinMaxClamp.y);

        target.localRotation = Quaternion.AngleAxis(cumulativeRotationX, Vector3.right);
    }

    private void DialHeldUpdate()
    {
        Vector2 currentMousePos = Input.mousePosition;

        Vector2 offset = new Vector2((MousePositionOrigin.x - currentMousePos.x + 0.2f) * 0.01f, (MousePositionOrigin.y - currentMousePos.y + 0.2f) * 0.01f);

        //Vector3 lookPosition = (dialForward * offset.x) + (-dialUp * offset.y);
        Vector3 lookPosition = (transform.forward * offset.x) + (-transform.up * offset.y);

        SetLookAtX(dialObject.transform, dialObject.transform.position + lookPosition, MinMaxCumulativeRotation);


    }

    private void Update()
    {
        if (!Held)
        {
            return;
        }

        DialHeldUpdate();
    }
}

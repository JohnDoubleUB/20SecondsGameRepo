using Unity.VisualScripting;
using UnityEngine;

public class RadioDisplay : MonoBehaviour
{
    [SerializeField]
    [ColorUsage(true, true)]
    private Color OffBackgroundColor = Color.grey;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color OnBackgroundColor = Color.red;

    private bool On;

    [SerializeField]
    Transform StartPoint;

    [SerializeField]
    Transform EndPoint;

    [SerializeField]
    Transform Needle;

    [SerializeField]
    private float NeedlePosition = 0.5f;

    private float needlePositionLastFrame = 0;

    [SerializeField]
    private MeshRenderer LightRenderer;

    private Material LightMaterial;

    private void Awake()
    {
        if (LightRenderer == null)
        {
            return;
        }

        LightMaterial = LightRenderer.material;
        SetLightColor(OffBackgroundColor);
    }

    private void SetLightColor(Color color)
    {
        LightMaterial.SetColor("_EmissionColor", color);
    }

    public void SetOn(bool on) 
    {
        On = on;
        SetLightColor(On ? OnBackgroundColor : OffBackgroundColor);
    }

    public float GetNeedlePosition() 
    {
        return NeedlePosition;
    }

    public void SetNeedlePosition(float needlePos)
    {
        NeedlePosition = Mathf.Clamp(needlePos, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(NeedlePosition == needlePositionLastFrame)
        {
            return;
        }

        Vector3 newPos = Vector3.Lerp(StartPoint.localPosition, EndPoint.localPosition, NeedlePosition);
        newPos.y = Needle.transform.localPosition.y;

        Needle.transform.localPosition = newPos;
    }
}

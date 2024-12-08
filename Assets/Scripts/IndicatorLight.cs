using UnityEngine;

public enum LightColor
{
    None,
    Red,
    Green,
    Blue,
    Yellow
}

public class IndicatorLight : MonoBehaviour
{
    public bool blink = true;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color NoneColor = Color.grey;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color RedColor = Color.red;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color GreenColor = Color.green;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color YellowColor = Color.yellow;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color BlueColor = Color.blue;

    [SerializeField]
    private MeshRenderer LightRenderer;

    private Material LightMaterial;

    private float interval = 0;

    public bool isOn { get; private set; }

    private void Awake()
    {
        if (LightRenderer == null) 
        {
            return;
        }

        LightMaterial = LightRenderer.material;
        SetLightColor(NoneColor);
    }

    private void SetLightColor(Color color) 
    {
        LightMaterial.SetColor("_EmissionColor", color);
    }

    private void Update()
    {
        if (!blink) 
        {
            return;
        }

        if (interval > 0) 
        {
            interval -= Time.deltaTime;
            return;
        }

        SetLightColor(NoneColor);
    }
    public LightColor StringToLight(char color)
    {
        switch (color)
        {
            case 'N':
                return LightColor.None;
            case 'R':
                return LightColor.Red;
            case 'G':
                return LightColor.Green;
            case 'B':
                return LightColor.Blue;
            case 'Y':
                return LightColor.Yellow;
        }

        return LightColor.None;
    }

    public void SetLight(char color, float interval) 
    {
        SetLight(StringToLight(color), interval);
    }
    public void SetLight(LightColor color, float interval)
    {
        this.interval = interval;
        switch (color)
        {
            case LightColor.None:
                SetLightColor(NoneColor);
                isOn = false;
                break;
            case LightColor.Red:
                SetLightColor(RedColor);
                isOn = true;
                break;
            case LightColor.Green:
                SetLightColor(GreenColor);
                isOn = true;
                break;
            case LightColor.Blue:
                SetLightColor(BlueColor);
                isOn = true;
                break;
            case LightColor.Yellow:
                SetLightColor(YellowColor);
                isOn = true;
                break;
        }
    }
}

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
    [SerializeField]
    private Material Red;
    [SerializeField]
    private Material Green;
    [SerializeField]
    private Material Blue;
    [SerializeField]
    private Material Yellow;
    [SerializeField]
    private Material None;

    [SerializeField]
    private MeshRenderer Renderer;

    private float interval = 0;

    private void Update()
    {
        if (interval > 0) 
        {
            interval -= Time.deltaTime;
            return;
        }
        
        if(Renderer.material != None) 
        {
            Renderer.material = None;
        }
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
                Renderer.material = None;
                break;
            case LightColor.Red:
                Renderer.material = Red;
                break;
            case LightColor.Green:
                Renderer.material = Green;
                break;
            case LightColor.Blue:
                Renderer.material = Blue;
                break;
            case LightColor.Yellow:
                Renderer.material = Yellow;
                break;
        }
    }
}

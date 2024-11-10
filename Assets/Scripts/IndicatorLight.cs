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

    public void SetLight(LightColor color)
    {
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

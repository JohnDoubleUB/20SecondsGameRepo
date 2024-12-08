using UnityEngine;

public class EnvironmentalEffectManager : MonoBehaviour
{
    public static EnvironmentalEffectManager current;

    [SerializeField]
    private ReactiveCageLight[] CageLights;

    private Material CageLightMaterial;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color CageLightDefaultColor = Color.white;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color CageLightMinColor = Color.white;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color CageLightMaxColor = Color.red;


    [SerializeField]
    private float SineFrequency = 5f;

    [SerializeField]
    private Light DirectionalLight;

    [SerializeField]
    private Color LightDefaultColor = Color.white;

    [SerializeField]    
    private Color LightColorMin = Color.white;

    [SerializeField]
    private Color LightColorMax = Color.red;

    [SerializeField]
    private ParticleSystem[] FogEffects;

    public bool EffectsEnabled { get; private set; } = false;

    private float Timer;

    public void EnableEffects(bool enable, bool clearParticlesImmediate = true) 
    {
        EffectsEnabled = enable;
        Timer = 0;
        EnableFog(enable, clearParticlesImmediate);
        DirectionalLight.color = LightDefaultColor;
        SetCageLightColor(CageLightDefaultColor);
    }

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;

        EnableFog(false);

        InitializeCageLights();
    }

    private void InitializeCageLights() 
    {
        if(CageLights.Length < 1) 
        {
            return;
        }

        CageLightMaterial = CageLights[0].Renderer.material;

        foreach(ReactiveCageLight cageLight in CageLights) 
        {
            cageLight.Renderer.material = CageLightMaterial;
        }

        SetCageLightColor(CageLightDefaultColor);
    }

    private void SetCageLightColor(Color color) 
    {
        CageLightMaterial.SetColor("_EmissionColor", color);
    }

    private void Update()
    {
        if (!EffectsEnabled) 
        {
            return;
        }

        Timer += Time.deltaTime * SineFrequency;

        float sineValue = Mathf.Sin(Timer);

        float normalizedValue = (sineValue + 1f) / 2f;

        DirectionalLight.color = Color.Lerp(LightColorMin, LightColorMax, normalizedValue);
        SetCageLightColor(Color.Lerp(CageLightMinColor, CageLightMaxColor, normalizedValue));
        //float currentValue = Mathf.Lerp(0, 1, normalizedValue);

    }

    private void EnableFog(bool enabled, bool clearParticlesImmediate = true) 
    {
        foreach(ParticleSystem system in FogEffects) 
        {
            if(clearParticlesImmediate) 
            {
                system.Clear();
            }

            if (enabled)
            {
                system.Play();
            }
            else 
            {
                system.Stop();
            }
        }
    }

    //private LinearColor LinearColorLerp(LinearColor a, LinearColor b, float t)
    //{

    //    t = Mathf.Clamp01(t);
    //    LinearColor newColor = new LinearColor();
    //    newColor.red = a.red + (b.red - a.red) * t;
    //    newColor.green = a.green + (b.green - a.green) * t;
    //    newColor.blue = a.blue + (b.blue - a.blue) * t;
    //    newColor.intensity = a.intensity + (b.intensity - a.intensity) * t;
    //    return newColor;
    //}
}

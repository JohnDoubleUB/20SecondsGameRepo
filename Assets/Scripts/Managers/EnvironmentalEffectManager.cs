using UnityEngine;

public class EnvironmentalEffectManager : MonoBehaviour
{
    public static EnvironmentalEffectManager current;

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

    public void EnableEffects(bool enable) 
    {
        EffectsEnabled = enable;
        Timer = 0;
        EnableFog(enable);
        DirectionalLight.color = LightDefaultColor;
    }

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;

        EnableFog(false);
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
        //float currentValue = Mathf.Lerp(0, 1, normalizedValue);

    }

    private void EnableFog(bool enabled) 
    {
        foreach(ParticleSystem system in FogEffects) 
        {
            if (enabled)
            {
                system.Clear();
                system.Play();
            }
            else 
            {
                system.Stop();
                system.Clear();
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

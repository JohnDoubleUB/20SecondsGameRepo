using UnityEngine;

public class ScreenEffectManager : MonoBehaviour
{
    public static ScreenEffectManager current;

    [SerializeField]
    private CRTEffectController CRTController;

    [SerializeField]
    private Vector2 CRTVignetteMinMax = new Vector2(0, 10);

    [SerializeField]
    private Vector2 ScreenBendMinMax = new Vector2(20, 0);

    [SerializeField]
    private float TransitionDuration = 2f; // Duration of the transition
    private float minValue = 0f; // Starting value
    private float maxValue = 1f; // Peak value


    private float elapsedTime = 0f; // Timer
    private bool transitionInProgress = false;

    void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

    public void PlayTransition() 
    {
        transitionInProgress = true;
        elapsedTime = TransitionDuration / 2;
    }

    private void CrtTransition(float value) 
    {
        if (CRTController == null)
        {
            return;
        }
        //CRTController.SetVignetteAmount(Mathf.Lerp(CRTVignetteMinMax.x, CRTVignetteMinMax.y, value));
        CRTController.SetScreenBend(Mathf.Lerp(ScreenBendMinMax.x, ScreenBendMinMax.y, value));
    }

    private void TransitionUpdate() 
    {
        if (!transitionInProgress)
        {
            return;
        }

        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate normalized progress (0 to 1)
        float t = Mathf.Clamp01(elapsedTime / TransitionDuration);

        // Map t to the sine wave (half-cycle: 0 -> 1 -> 0)
        float sineValue = Mathf.Sin(t * Mathf.PI);

        // Scale sineValue between minValue and maxValue
        float currentValue = Mathf.Lerp(minValue, maxValue, sineValue);

        CrtTransition(currentValue);

        // Reset elapsedTime when transition is complete (for looping)
        if (elapsedTime >= TransitionDuration)
        {
            elapsedTime = TransitionDuration; // Reset timer to repeat the transition
            transitionInProgress = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TransitionUpdate();
    }
}

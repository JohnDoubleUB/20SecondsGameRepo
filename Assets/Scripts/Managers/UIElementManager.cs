using TMPro;
using UnityEngine;

public class UIElementManager : MonoBehaviour
{
    public static UIElementManager current;

    public TextMeshProUGUI TimerText; 

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

    private void Update()
    {
        if (TimerText != null)
        {
            TimerText.text = $"Time: {System.Math.Round(GameManager.current.CurrentTime, 2)}";
        }
    }
}

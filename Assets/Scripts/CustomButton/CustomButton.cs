using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

[RequireComponent(typeof(RawImage))]
public class CustomButton : UIBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler
{
    [SerializeField]
    private Texture2D Default;

    [SerializeField] 
    private Texture2D Pressed;

    [SerializeField]
    private Color DefaultColor;

    [SerializeField] 
    private Color PressedColor;

    [SerializeField]
    private string HoverText = "PLAY";

    [SerializeField]
    private RawImage PrimaryImage;

    [SerializeField]
    private RawImage SecondaryImage;

    [SerializeField]
    private float BlinkRate = 5.0f;

    private float CurrentBlinkTimer = 0f;

    private bool Blinking = false;

    private bool IsHovered = false;

    private bool IsPressed = false;

    [SerializeField]
    private TextMeshProUGUI PausePlayText;

    [SerializeField]
    private ButtonClickedEvent m_OnDelayedClick = new ButtonClickedEvent();

    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    private void Start()
    {
        PausePlayText.text = "";
    }
    private void Reset()
    {
        PrimaryImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        if (IsHovered && !IsPressed) 
        {
            return;
        }

        if (CurrentBlinkTimer <= 0f)
        {
            SetBlink(!Blinking);
            CurrentBlinkTimer = BlinkRate;

            if (IsPressed) 
            {
                IsPressed = false;
                PrimaryImage.texture = Default;
                SecondaryImage.texture = Default;
                PrimaryImage.color = DefaultColor;
                m_OnDelayedClick?.Invoke();
            }

            return;
        }

        CurrentBlinkTimer -= Time.deltaTime;

    }

    private void SetBlink(bool value) 
    {
        Blinking = value;
        PrimaryImage.enabled = Blinking;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsPressed) 
        {
            return;
        }
        SetBlink(true);
        PrimaryImage.texture = Pressed;
        SecondaryImage.texture = Pressed;
        PrimaryImage.color = PressedColor;
        CurrentBlinkTimer = BlinkRate * 2;
        
        m_OnClick?.Invoke();
        IsPressed = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PausePlayText.text = HoverText;
        PrimaryImage.texture = Pressed;
        SecondaryImage.texture = Pressed;
        IsHovered = true;
        SetBlink(true);
        CurrentBlinkTimer = 0;
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PausePlayText.text = "";

        IsHovered = false;

        if (!IsPressed) 
        {
            PrimaryImage.texture = Default;
            SecondaryImage.texture = Default;
        }
        //CurrentBlinkTimer = 0;
        //throw new System.NotImplementedException();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}

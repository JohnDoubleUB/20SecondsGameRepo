using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

[RequireComponent(typeof(RawImage))]
public class CustomButton : UIBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler
{
    [SerializeField]
    private TextMeshProUGUI Text;

    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    [SerializeField]
    private Color DefaultColor = Color.black;

    [SerializeField]
    private Color HighlightColor = Color.yellow;

    [SerializeField]
    private Color PressBodyColor = Color.yellow;

    [SerializeField]
    private Color DefaultBodyColor = Color.white;

    [SerializeField]
    private float outlineHoverWidth = 0.07f;

    [SerializeField]
    private float BodyHighlightTimer = 0.2f;

    [SerializeField]
    private bool PlayScreenTransitionEffectOnOver = true;

    private void Start()
    {
        Text.outlineWidth = outlineHoverWidth;
        Text.outlineColor = DefaultColor;
        Text.color = DefaultBodyColor;
    }

    private void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_OnClick?.Invoke();
        Text.color = DefaultBodyColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Text.outlineWidth = outlineHoverWidth;
        Text.outlineColor = HighlightColor;
        Text.color = PressBodyColor;

        if(PlayScreenTransitionEffectOnOver) 
        {
            ScreenEffectManager.current.PlayTransition();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Text.outlineWidth = 0.03f;
        Text.outlineColor = DefaultColor;
        Text.color = DefaultBodyColor;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ItemText;

    [SerializeField]
    private Image ItemImage;

    [SerializeField]
    private Image BackgroundImage;

    [SerializeField]
    private Color SelectedBackgroundColor = Color.yellow;

    [SerializeField]
    private Color UnselectedBackgroundColor = Color.black;

    public PickedUpItem Item { get; private set; }

    public bool IsSelected { get; private set; } = false;

    public void SetSelected(bool selected) 
    {
        IsSelected = selected;
        BackgroundImage.color = IsSelected ? SelectedBackgroundColor : UnselectedBackgroundColor;
    }

    public void Initialize(PickedUpItem pickedUpItem) 
    {
        Item = pickedUpItem;
        ItemImage.sprite = pickedUpItem.Icon;
        ItemText.text = pickedUpItem.Name;
    }
}

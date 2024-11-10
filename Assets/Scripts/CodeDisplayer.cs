using TMPro;
using UnityEngine;

public class CodeDisplayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Text;

    public bool startVisible = true;

    private void Awake()
    {
        if(Text != null) 
        {
            Text.gameObject.SetActive(startVisible);
        }
    }
    public void Show()
    {
        Text.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Text.gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        if (Text == null) 
        {
            return;
        }

        Text.text = text;
    }
}

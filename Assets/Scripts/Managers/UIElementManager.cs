using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIElementManager : MonoBehaviour
{
    public static UIElementManager current;

    public TextMeshProUGUI TimerText;

    public TextMeshProUGUI ItemText;

    public TextMeshProUGUI CodeText;
    public TextMeshProUGUI CodeText2;

    [SerializeField]
    private Image DeathStatic;

    [SerializeField]
    private GameObject ItemDisplayParent;

    [SerializeField]
    private UIItem UIItemPrefab;

    private List<UIItem> SpawnedUIItems = new List<UIItem>();

    public char PasswordGapCharacter = '_';

    private string CurrentCode = string.Empty;

    public Color DeathStaticColor;

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

    public void ToggleCodeDisplayCenter(bool center)
    {
        CodeText.gameObject.SetActive(!center);
        CodeText2.gameObject.SetActive(center);
    }

    private void SetStaticColor(float value)
    {
        Color newColor = new Color(DeathStaticColor.r, DeathStaticColor.g, DeathStaticColor.b, value);
        DeathStatic.color = newColor;
    }

    private void UpdateStaticColor() 
    {
        SetStaticColor(Mathf.Clamp(GameManager.current.DeathEffectAmount, 0, 0.95f));
    }

    private void Update()
    {
        if (TimerText != null)
        {
            TimerText.text = $"Time: {System.Math.Round(GameManager.current.CurrentTime, 2)}";
        }

        UpdateStaticColor();

        if (GameManager.current.SessionData == null)
        {
            if (!string.IsNullOrEmpty(CurrentCode)) 
            {
                CurrentCode = string.Empty;
            }
            
            return;
        }

        if (CurrentCode != GameManager.current.SessionData.CurrentCode) 
        {
            CurrentCode = GameManager.current.SessionData.CurrentCode;
            
            string value = CurrentCode.PadRight(
                    GameManager.current.SessionData.CodeCount * GameManager.current.SessionData.CodeLength,
                    PasswordGapCharacter
                    );



            CodeText.text = value;
            CodeText2.text = value;
        }
    }

    private void Start()
    {
        if (ItemText != null) 
        {
            ItemText.text = "";
        }

        ToggleCodeDisplayCenter(false);

        if (PlayerController.current == null)
        {
            return;
        }

        PlayerController.current.OnItemsHeldUpdate += OnItemsHeldUpdate;
        PlayerController.current.OnResetInventory += OnResetInventory;

        if (PlayerController.current.PickupInteractor == null)
        {
            return;
        }

        PlayerController.current.PickupInteractor.OnInteractingItemUpdate += OnInteractingItemUpdate;
    }

    public void PlayGame() 
    {
        GameManager.current.StartGame();
    }

    private void OnResetInventory()
    {
        DeleteAllHeldItems();
    }

    private void OnInteractingItemUpdate(string interactingItemName)
    {
        if (ItemText == null) 
        {
            return;
        }

        ItemText.text = interactingItemName;
    }

    private void SpawnNewUIItem(PickedUpItem item) 
    {
        if (UIItemPrefab == null) 
        {
            return;
        }

        UIItem newInstance = Instantiate(UIItemPrefab, ItemDisplayParent.transform);

        newInstance.Initialize(item);
        SpawnedUIItems.Add(newInstance);
    }

    private void OnItemsHeldUpdate(PickedUpItem item, bool pickupOrDrop)
    {
        //This is where the items that are picked up would be made to appear in the hud OR be removed from it

        if (pickupOrDrop) 
        {
            SpawnNewUIItem(item);
            return;
        }


        for (int i = 0; i < SpawnedUIItems.Count; i++)
        {
            if (item.Index == SpawnedUIItems[i].Item.Index) 
            {
                Destroy(SpawnedUIItems[i].gameObject);
                SpawnedUIItems.RemoveAt(i);
                break;
            }
        }
    }

    public void DeleteAllHeldItems() 
    {
        for (int i = 0; i < SpawnedUIItems.Count; i++)
        {
            UIItem item = SpawnedUIItems[i];
            
            if (item == null)
            {
                continue;
            }

            Destroy(item.gameObject);
        }

        SpawnedUIItems.Clear();
    }

    public void Quit() 
    {
        //TODO: Implement return to menu
    }

    private void OnDestroy()
    {
        if(PlayerController.current == null) 
        {
            return;
        }

        PlayerController.current.OnItemsHeldUpdate -= OnItemsHeldUpdate;
        PlayerController.current.OnResetInventory -= OnResetInventory;

        if (PlayerController.current.PickupInteractor == null) 
        {
            return;
        }

        PlayerController.current.PickupInteractor.OnInteractingItemUpdate -= OnInteractingItemUpdate;
    }
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIElementManager : MonoBehaviour
{
    public static UIElementManager current;

    public TextMeshProUGUI TimerText;

    public TextMeshProUGUI ItemText;

    public TextMeshProUGUI CodeText;

    [SerializeField]
    private GameObject ItemDisplayParent;

    [SerializeField]
    private UIItem UIItemPrefab;

    private List<UIItem> SpawnedUIItems = new List<UIItem>();

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

    private void Start()
    {
        if (ItemText != null) 
        {
            ItemText.text = "";
        }

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

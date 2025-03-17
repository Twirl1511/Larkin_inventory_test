using UnityEngine;
using System.Collections.Generic;

public class BackpackButtonsManager : MonoBehaviour
{
    [SerializeField] private List<ItemsButton> _itemButtons;
    [SerializeField] private Inventory _inventory;


    private void Start()
    {
        UpdateButtonsHandle();
        _inventory.OnInventoryChanged.AddListener(UpdateButtonsHandle);

        foreach (var item in _itemButtons)
            item.OnClicked += ClickHandler;
    }

    private void OnDestroy()
    {
        _inventory.OnInventoryChanged.RemoveListener(UpdateButtonsHandle);

        foreach (var item in _itemButtons)
            item.OnClicked -= ClickHandler;
    }

    private void ClickHandler(ItemSlot itemSlot)
    {
        _inventory.TryTakeItemOut(itemSlot);
    }

    private void UpdateButtonsHandle()
    {
        foreach (var button in _itemButtons)
            button.UpdateSlot(button.ItemSlot.IsOcupied);
    }
}

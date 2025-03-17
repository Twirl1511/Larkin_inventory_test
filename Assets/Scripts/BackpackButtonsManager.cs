using UnityEngine;
using System.Collections.Generic;

public class BackpackButtonsManager : MonoBehaviour
{
    [SerializeField] private List<ItemsButton> _itemButtons;
    [SerializeField] private Inventory _inventory;


    private void Start()
    {
        UpdateButtonsHandle();
        _inventory.OnInventoryChanged += UpdateButtonsHandle;

        foreach (var item in _itemButtons)
            item.OnClicked += ClickHandler;
    }

    private void OnDestroy()
    {
        _inventory.OnInventoryChanged -= UpdateButtonsHandle;

        foreach (var item in _itemButtons)
            item.OnClicked -= ClickHandler;
    }

    private void ClickHandler(ItemSlot itemSlot)
    {
        if (!_inventory.TryTakeOutItem(itemSlot, out Item item))
            return;

        DragAndDropSystem.Instance.PickUpObject(item);
    }

    private void UpdateButtonsHandle()
    {
        foreach (var button in _itemButtons)
            button.UpdateSlot(button.ItemSlot.IsOcupied);
    }
}

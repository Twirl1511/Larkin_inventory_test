using System;
using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> _slots;

    public event Action OnInventoryChanged;


    public void PlaceIn(Item item)
    {
        foreach (var slot in _slots)
        {
            if(slot.TryPlaceInSlot(item))
                OnInventoryChanged?.Invoke();
        }
    }

    public bool TryTakeOutItem(ItemSlot itemSlot, out Item item)
    {
        if (!itemSlot.TryGetItemFromSlot(out item))
            return false;

        OnInventoryChanged?.Invoke();
        return true;
    }
}

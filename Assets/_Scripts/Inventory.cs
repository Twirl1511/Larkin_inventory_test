using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> _slots;

    public UnityEvent OnInventoryChanged;


    public void PlaceIn(Item item)
    {
        foreach (var slot in _slots)
        {
            if (!slot.TryPlaceInSlot(item))
                continue;

            InventoryApi.Instance.SendItemStatus(item.Id, "Added");
            OnInventoryChanged?.Invoke();
            break;
        }
    }

    public void TryTakeItemOut(ItemSlot itemSlot)
    {
        if (!itemSlot.TryGetItemFromSlot(out Item item))
            return;

        InventoryApi.Instance.SendItemStatus(item.Id, "Removed");
        OnInventoryChanged?.Invoke();
        return;
    }
}

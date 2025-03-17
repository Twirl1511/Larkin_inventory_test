using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> _slots;

    public UnityEvent OnInventoryChanged;


    public async void PlaceIn(Item item)
    {
        foreach (var slot in _slots)
        {
            if (!slot.TryPlaceInSlot(item))
                continue;

            await InventoryApi.Instance.SendItemStatusAsync(item.Id, "Added");
            OnInventoryChanged?.Invoke();
            break;
        }
    }

    public async Task TryTakeItemOut(ItemSlot itemSlot)
    {
        if (!itemSlot.TryGetItemFromSlot(out Item item))
            return;

        await InventoryApi.Instance.SendItemStatusAsync(item.Id, "Removed");
        OnInventoryChanged?.Invoke();
        return;
    }
}

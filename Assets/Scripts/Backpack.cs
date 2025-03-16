using UnityEngine;

public class Backpack : MonoBehaviour
{
    [SerializeField] private ItemSlot[] _slots;


    public void PlaceIn(Item item)
    {
        foreach (var slot in _slots)
        {
            slot.TryPlaceInSlot(item);
        }
    }
}

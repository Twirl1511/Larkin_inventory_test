using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [SerializeField] private ItemSlot[] _slots;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Item item))
            return;

        PlaceIn(item);
    }

    private void PlaceIn(Item item)
    {
        foreach (var slot in _slots)
        {
            slot.TryPlaceInSlot(item);
        }
    }
}

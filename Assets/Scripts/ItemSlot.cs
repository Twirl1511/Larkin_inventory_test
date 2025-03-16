using DG.Tweening;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private EItemType _type;
    [SerializeField] private float _spapTime = 0.7f;

    public bool IsOcupied { get; private set; }
    private Item _item;


    public bool TryPlaceInSlot(Item item)
    {
        if (IsOcupied)
            return false;

        if (_type != item.Type)
            return false;

        IsOcupied = true;
        _item = item;
        SetInSlotPosition(item);

        return true;
    }

    public bool TryGetItemFromSlot(out Item item)
    {
        item = null;
        if (_item == null)
            return false;

        item = _item;
        Release(_item);

        return true;
    }

    private void Release(Item item)
    {
        IsOcupied = false;

        item.Rigidbody.isKinematic = true;
        item.Rigidbody.useGravity = false;
        item.Rigidbody.velocity = Vector3.zero;
        item.Rigidbody.angularVelocity = Vector3.zero;

        _item = null;
    }

    private void SetInSlotPosition(Item item)
    {
        item.Place();

        item.Rigidbody.isKinematic = true; 
        item.Rigidbody.useGravity = false; 
        item.Rigidbody.velocity = Vector3.zero; 
        item.Rigidbody.angularVelocity = Vector3.zero;

        SnapToSlot(item, _spapTime);
    }

    public void SnapToSlot(Item item, float duration)
    {
        item.transform.SetParent(transform);

        item.transform.DOLocalMove(Vector3.zero, duration).SetEase(Ease.OutQuad);
        item.transform.DOLocalRotateQuaternion(Quaternion.identity, duration).SetEase(Ease.OutQuad);
    }
}

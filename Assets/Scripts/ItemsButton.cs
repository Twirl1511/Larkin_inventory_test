using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemsButton : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private ItemSlot _itemSlot;
    [SerializeField] private Image _image;
    [SerializeField] private string _name;

    public ItemSlot ItemSlot => _itemSlot;
    
    private bool _isPointerDown;

    public event Action<ItemSlot> OnClicked;


    private void Update()
    {
        if (_isPointerDown && Input.GetMouseButtonUp(0))
        {
            OnPointerDown(new PointerEventData(EventSystem.current));
            OnPointerUp(new PointerEventData(EventSystem.current));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerDown = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerDown = false;
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;

        if (!_button.interactable)
            return;

        OnClicked.Invoke(_itemSlot);
    }

    public void UpdateSlot(bool isOcupied)
    {
        _button.interactable = isOcupied;
    }
}

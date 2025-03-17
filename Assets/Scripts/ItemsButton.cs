using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

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
            OnPointerUp(new PointerEventData(EventSystem.current));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerDownHandler);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerDown = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;

        if (!_button.interactable)
            return;

        OnClicked.Invoke(_itemSlot);
        Debug.Log(2222);
    }

    public void UpdateSlot(bool isOcupied)
    {
        _button.interactable = isOcupied;
    }
}

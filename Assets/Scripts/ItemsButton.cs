using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private EItemType _itemType;
    [SerializeField] private Image _image;
    [SerializeField] private string _name;
    [SerializeField, Range(0,1)] private float _alphaNotOcupied;


    public void Show(bool isOcupied)
    {
        SetAlpha(isOcupied ? 1 : _alphaNotOcupied);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _image.color;
        color.a = Mathf.Clamp01(alpha);
        _image.color = color;
    }

}

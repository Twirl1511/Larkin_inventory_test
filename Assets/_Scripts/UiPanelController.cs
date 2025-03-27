using DG.Tweening;
using UnityEngine;

public class UiPanelController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private float _openTime = 1f;
    [SerializeField] private float _closeTime = 0.5f;

    private Tween _currentTween;
    private bool _isOpen;


    private void Start()
    {
        _canvas.gameObject.SetActive(false);
        _canvas.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && _isOpen)
            ToggleMenu(false);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !_isOpen)
            ToggleMenu(true);
    }

    private void ToggleMenu(bool isOpening)
    {
        if (_currentTween != null && _currentTween.IsActive())
            _currentTween.Kill(false);

        if (isOpening)
            Open();
        else
            Close();
    }

    private void Open()
    {
        _isOpen = true;
        if (!_canvas.gameObject.activeSelf)
            _canvas.gameObject.SetActive(true);

        _currentTween = _canvas.transform
            .DOScale(Vector3.one, _openTime)
            .SetEase(Ease.OutBack);
    }

    private void Close()
    {
        _isOpen = false;
        _currentTween = _canvas.transform
            .DOScale(Vector3.zero, _closeTime)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                _canvas.gameObject.SetActive(false);
            });
    }
}

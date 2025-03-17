using DG.Tweening;
using UnityEngine;

public class UiPanelController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private float _openTime = 1f;
    [SerializeField] private float _closeTime = 0.5f;

    private Tween _currentTween;
    private Camera _camera;


    private void Start()
    {
        _canvas.gameObject.SetActive(false);
        _canvas.transform.localScale = Vector3.zero;
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (!_canvas.gameObject.activeSelf)
            return;

        if (_camera != null)
            _canvas.transform.LookAt(_camera.transform);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            ToggleMenu(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            ToggleMenu(false);
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
        if (!_canvas.gameObject.activeSelf)
            _canvas.gameObject.SetActive(true);

        _currentTween = _canvas.transform
            .DOScale(Vector3.one, _openTime)
            .SetEase(Ease.OutBack);
    }

    private void Close()
    {
        _currentTween = _canvas.transform
               .DOScale(Vector3.zero, _closeTime)
               .SetEase(Ease.InBack)
               .OnComplete(() =>
               {
                   _canvas.gameObject.SetActive(false);
               });
    }
}

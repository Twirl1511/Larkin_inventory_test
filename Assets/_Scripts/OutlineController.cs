using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private Outline _outline;
    
    private bool _isActive = true;
    private bool _isAlwaysShow = false;


    private void Start()
    {
        Hide();
    }

    private void OnMouseEnter()
    {
        if (!_isActive)
            return;

        Show();
    }

    private void OnMouseExit()
    {
        if (!_isActive)
            return;

        if (_isAlwaysShow)
            return;

        Hide();
    }

    public void SetAlwaysShow(bool value)
    {
        _isAlwaysShow = value;
    }

    public void Show()
    {
        _outline.enabled = true;
    }

    public void Hide()
    {
        _outline.enabled = false;
    }
}

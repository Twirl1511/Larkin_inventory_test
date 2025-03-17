using UnityEngine;

public class DragAndDropSystem : MonoBehaviour
{
    private const int LEFT_MOUSE_BUTTON = 0;
    public static DragAndDropSystem Instance { get; private set; }

    [Header("Layers")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _dragableMask;

    [Header("Stats")]
    [SerializeField] private float _pickupDistance = 5f;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _heldDrag = 10f;
    [SerializeField] private float _releasedDrag = 1f;
    [SerializeField] private float _fixedHeight = 1.5f;
    [SerializeField] private float _throwForceMultiplier = 0.01f;

    private Camera _mainCamera;
    private Rigidbody _heldObject;
    private bool _isHoldingObject = false;
    private Vector3 _previousPosition;
    private Vector3 _velocity;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
            TryPickUpObject();

        if (_isHoldingObject)
        {
            MoveObjectWithMouse();

            if (Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON))
                ReleaseObject();
        }
    }

    private void TryPickUpObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, _pickupDistance, _dragableMask))
            return;

        if (!hit.transform.root.TryGetComponent(out Rigidbody rigidbody))
            return;

        _heldObject = rigidbody;
        _heldObject.useGravity = false;
        _heldObject.drag = _heldDrag;

        _previousPosition = _heldObject.position;

        _isHoldingObject = true;
        Cursor.visible = false;

        // highlight the object when picked up
        if (_heldObject.TryGetComponent(out OutlineController outlineController))
            outlineController.SetAlwaysShow(true);
    }

    private void MoveObjectWithMouse()
    {
        if (_heldObject == null)
            return;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask))
            return;

        // getting position over the floor
        Vector3 targetPosition = hit.point;
        targetPosition.y = _fixedHeight;

        MoveWithVelocity();
        RestrictVelocity();


        return;
        void MoveWithVelocity()
        {
            _velocity = (targetPosition - _previousPosition) / Time.deltaTime;
            _previousPosition = _heldObject.position;
            Vector3 moveDirection = (targetPosition - _heldObject.position);
            _heldObject.velocity = moveDirection * _moveSpeed;
        }
        void RestrictVelocity()
        {
            if (_heldObject.velocity.magnitude > _moveSpeed)
                _heldObject.velocity = _heldObject.velocity.normalized * _moveSpeed;
        }
    }

    private void ReleaseObject(bool isWithImpulse = true)
    {
        if (_heldObject == null)
            return;

        TryRemoveHighlight();

        _heldObject.useGravity = true;
        _heldObject.drag = _releasedDrag;

        AddImpusleAfterReleasing();
        TryPlaceInSlot();

        _heldObject = null;
        _isHoldingObject = false;
        Cursor.visible = true;


        return;
        void AddImpusleAfterReleasing()
        {
            if (isWithImpulse)
                _heldObject.AddForce(_velocity * _throwForceMultiplier, ForceMode.Impulse);
        }
        void TryPlaceInSlot()
        {
            if (_heldObject.TryGetComponent(out IPlaceable placeable))
                placeable.TryPlace();
        }
        void TryRemoveHighlight()
        {
            if (_heldObject.TryGetComponent(out OutlineController outlineController))
            {
                outlineController.SetAlwaysShow(false);
                outlineController.Hide();
            }
        }
    }
}

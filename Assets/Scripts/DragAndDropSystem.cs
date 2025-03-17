using UnityEngine;

public class DragAndDropSystem : MonoBehaviour
{
    public static DragAndDropSystem Instance { get; private set; }

    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _dragableMask;
    [SerializeField] private float _pickupDistance = 5f;
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _heldDrag = 10f;
    [SerializeField] private float _releasedDrag = 1f;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _fixedHeight = 1.5f;
    [SerializeField] private float _throwForceMultiplier = 5f;
    [SerializeField] private float _minDistanceFromCamera;

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
        if (Input.GetMouseButtonDown(0))
        {
            TryPickUpObject();
        }

        if (_isHoldingObject)
        {
            RotateObject();
            MoveObjectWithMouse();

            if (Input.GetMouseButtonUp(0))
                ReleaseObject();
        }
    }

    private void TryPickUpObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, _pickupDistance, _dragableMask))
            return;

        Rigidbody rigidbody = hit.transform.root.GetComponent<Rigidbody>();

        if (rigidbody == null)
            return;

        _heldObject = rigidbody;
        _heldObject.useGravity = false;
        _heldObject.drag = _heldDrag;

        _previousPosition = _heldObject.position;
        _isHoldingObject = true;
        Cursor.visible = false;

        OutlineController outlineController = _heldObject.GetComponent<OutlineController>();
        outlineController.SetAlwaysShow(true);
    }

    private void RotateObject()
    {
        if (_heldObject == null)
            return;

        float rotationY = 0f;

        if (Input.GetKey(KeyCode.Q))
            rotationY = -_rotationSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.E))
            rotationY = _rotationSpeed * Time.deltaTime;

        Quaternion yRotation = Quaternion.Euler(0, rotationY, 0);
        _heldObject.transform.rotation = yRotation * _heldObject.transform.rotation;
    }

    private void MoveObjectWithMouse()
    {
        if (_heldObject == null)
            return;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask))
            return;

        Vector3 targetPosition = hit.point;
        targetPosition.y = _fixedHeight;

        _velocity = (targetPosition - _previousPosition) / Time.deltaTime;

        float distanceFromCamera = Vector3.Distance(_mainCamera.transform.position, _heldObject.position);

        if (distanceFromCamera < _minDistanceFromCamera)
            targetPosition = _previousPosition;

        _previousPosition = _heldObject.position;
        Vector3 moveDirection = (targetPosition - _heldObject.position);
        _heldObject.velocity = moveDirection * _moveSpeed;
    }

    private void ReleaseObject(bool isWithImpulse = true)
    {
        if (_heldObject == null)
            return;

        OutlineController outlineController = _heldObject.GetComponent<OutlineController>();
        outlineController.SetAlwaysShow(false);
        outlineController.Hide();

        _heldObject.useGravity = true;
        _heldObject.drag = _releasedDrag;

        if(isWithImpulse)
            _heldObject.AddForce(_velocity * _throwForceMultiplier, ForceMode.Impulse);

        if (_heldObject.TryGetComponent(out IPlaceable placeable))
            placeable.TryPlace();

        _heldObject = null;
        _isHoldingObject = false;
        Cursor.visible = true;
    }
}

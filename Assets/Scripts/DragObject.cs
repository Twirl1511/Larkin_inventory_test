using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _dragableMask;
    [SerializeField] private Transform _virtualHand;
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
    private IPlaceable _placeable;


    private void Start()
    {
        _mainCamera = Camera.main;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPickupObject();
        }

        if (_isHoldingObject)
        {
            RotateObject();
            MoveObjectWithMouse();

            if (Input.GetMouseButtonUp(0))
            {
                ReleaseObject(true);
            }
        }
    }

    private void TryPickupObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _pickupDistance, _dragableMask))
        {
            Rigidbody rb = hit.transform.root.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                _heldObject = rb;
                _heldObject.useGravity = false;
                _heldObject.drag = _heldDrag;

                _previousPosition = _heldObject.position; 
                _isHoldingObject = true;
                Cursor.visible = false; 

                if(TryGetComponent(out IPlaceable placeable))
                {
                    if(_placeable != null)
                        _placeable.OnPlaced -= PlaceHandle;

                    _placeable = placeable;
                    _placeable.OnPlaced += PlaceHandle;
                }

                OutlineController outlineController = _heldObject.GetComponent<OutlineController>();
                outlineController.SetAlwaysShow(true);
            }
        }
    }

    private void PlaceHandle()
    {
        if(_placeable != null)
            _placeable.OnPlaced -= PlaceHandle;

        _placeable = null;
        ReleaseObject(false);
    }

    private void RotateObject()
    {
        if (_heldObject == null)
        {
            return;
        }

        float rotationY = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationY = -_rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationY = _rotationSpeed * Time.deltaTime;
        }

        Quaternion yRotation = Quaternion.Euler(0, rotationY, 0);
        _heldObject.transform.rotation = yRotation * _heldObject.transform.rotation;
    }

    private void MoveObjectWithMouse()
    {
        if (_heldObject == null)
        {
            return;
        }

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = _fixedHeight;

            _velocity = (targetPosition - _previousPosition) / Time.deltaTime;

            float distanceFromCamera = Vector3.Distance(_mainCamera.transform.position, _heldObject.position);

            if (distanceFromCamera < _minDistanceFromCamera)
            {
                targetPosition = _previousPosition;
            }

            _previousPosition = _heldObject.position;
            Vector3 moveDirection = (targetPosition - _heldObject.position);
            _heldObject.velocity = moveDirection * _moveSpeed;


            if (_virtualHand != null)
            {
                _virtualHand.position = _heldObject.position;
            }
        }
    }

    private void ReleaseObject(bool isWithImpulse)
    {
        if (_heldObject == null)
        {
            return;
        }

        OutlineController outlineController = _heldObject.GetComponent<OutlineController>();
        outlineController.SetAlwaysShow(false);
        outlineController.Hide();

        _heldObject.useGravity = true;
        _heldObject.drag = _releasedDrag;

        if(isWithImpulse)
            _heldObject.AddForce(_velocity * _throwForceMultiplier, ForceMode.Impulse);

        _heldObject = null;
        _isHoldingObject = false;
        Cursor.visible = true;

        if (_placeable != null)
            _placeable.OnPlaced -= PlaceHandle;

        _placeable = null;
    }
}

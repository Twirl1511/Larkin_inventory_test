using System;
using UnityEngine;

public class Item : MonoBehaviour, IPlaceable
{
    private const float PLACE_RADIUS_SPHERE = 0.2f;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private EItemType _type;
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private float _weight;

    public event Action OnPlaced;
    public event Action OnPickedUp;

    public string Id => _id;
    public string Name => _name;
    public float Weight => _weight;
    public EItemType Type => _type;
    public Rigidbody Rigidbody => _rigidbody;


    private void Start()
    {
        _rigidbody.mass = _weight;
    }

    public void TryPlace()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, PLACE_RADIUS_SPHERE);
        foreach (Collider collider in colliders)
        {
            if(collider.TryGetComponent(out Inventory backpack))
                backpack.PlaceIn(this);
        }
    }
}

using System;
using UnityEngine;

public class Item : MonoBehaviour, IPlaceable
{
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


    public void TryPlace()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);
        foreach (Collider collider in colliders)
        {
            if(collider.TryGetComponent(out Inventory backpack))
            {
                backpack.PlaceIn(this);
                Debug.Log("backpack.PlaceIn(this);");
            }
        }
    }
}

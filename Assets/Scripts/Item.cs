using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private string _id;
    private string _name;
    private float _weight;

    public string Id => _id;
    public string Name => _name;
    public float Weight => _weight;


    public void Init(string id, string name, float weight)
    {
        _id = id;
        _name = name;
        _weight = weight;
        _rigidbody.mass = weight;
    }
}

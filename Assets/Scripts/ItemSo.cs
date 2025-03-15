using UnityEngine;

[CreateAssetMenu(fileName = "Item_name", menuName = "ScriptableObjects/Item")]
public class ItemSo : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [Min(0.1f)]
    [SerializeField] private float _weight;

    public string Id => _id;
    public string Name => _name;
    public float Weight => _weight;
}

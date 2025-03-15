using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public async Task<Item> CreateItem(ItemSo itemSo, Vector3 position)
    {
        GameObject instance = await Addressables.InstantiateAsync(itemSo.Id, position, Quaternion.identity).Task;
        Item item = instance.GetComponent<Item>();
        item.Init(itemSo.Id, itemSo.Name, itemSo.Weight);

        Debug.Log($"{itemSo.Name} has created at position {position}");

        return item;
    }
}

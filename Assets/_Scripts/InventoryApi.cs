using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class InventoryApi : MonoBehaviour
{
    public static InventoryApi Instance { get; private set; }

    private const string API_URL = "https://wadahub.manerai.com/api/inventory/status";
    private const string AUTH_TOKEN = "Bearer kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public async Task SendItemStatusAsync(string itemId, string eventType)
    {
        await SendRequest(itemId, eventType);
    }

    private async Task SendRequest(string itemId, string eventType)
    {
        string jsonData = $"{{\"item_id\":\"{itemId}\",\"event\":\"{eventType}\"}}";
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(API_URL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AUTH_TOKEN);

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
                Debug.Log($"[Success] Item {itemId} event {eventType} sent.");
            else
                Debug.LogError($"[Error] Failed to send item {itemId}. Error: {request.error}");
        }
    }
}

using UnityEngine;
using System.Text;
using System.Collections;
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

    public void SendItemStatus(string itemId, string eventType)
    {
        StartCoroutine(SendRequest(itemId, eventType));
    }

    private IEnumerator SendRequest(string itemId, string eventType)
    {
        string jsonData = $"{{\"item_id\":\"{itemId}\",\"event\":\"{eventType}\"}}";
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(API_URL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AUTH_TOKEN);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                Debug.Log($"[Server Response] {request.downloadHandler.text}");
            else
                Debug.LogError($"[Error] Failed to send item {itemId}. Error: {request.error}");
        }
    }
}

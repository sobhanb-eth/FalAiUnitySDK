// Runtime/Scripts/Utils/TripoSRApi.cs
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text;

namespace TripoSR.SDK
{
    public class TripoSRApi
{
    private readonly string apiKey;
    private const string API_BASE_URL = "https://queue.fal.run/fal-ai/triposr";

    public TripoSRApi(TripoSRConfig config)
    {
        this.apiKey = config.ApiKey;
    }

    public async Task<string> SubmitRequest(TripoSRRequest request)
    {
        // Enqueue the request
        string requestPayload = JsonUtility.ToJson(request);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(requestPayload);

        using var enqueueRequest = new UnityWebRequest(API_BASE_URL, "POST");
        enqueueRequest.uploadHandler = new UploadHandlerRaw(payloadBytes);
        enqueueRequest.downloadHandler = new DownloadHandlerBuffer();
        enqueueRequest.SetRequestHeader("Authorization", $"Key {apiKey}");
        enqueueRequest.SetRequestHeader("Content-Type", "application/json");

        await enqueueRequest.SendWebRequest();

        if (enqueueRequest.result != UnityWebRequest.Result.Success)
        {
            throw new TripoSRException($"Failed to enqueue request: {enqueueRequest.error}");
        }

        var enqueueResponse = JsonUtility.FromJson<QueueResponse>(enqueueRequest.downloadHandler.text);
        string requestId = enqueueResponse.request_id;

        // Poll status
        string statusUrl = $"{API_BASE_URL}/requests/{requestId}/status";
        string responseUrl = await PollStatus(statusUrl);

        // Fetch the result
        return await FetchResult(responseUrl);
    }

    private async Task<string> PollStatus(string statusUrl)
    {
        bool isCompleted = false;
        string responseUrl = string.Empty;

        do
        {
            await Task.Delay(5000); // Poll every 5 seconds
            using var statusRequest = UnityWebRequest.Get(statusUrl);
            statusRequest.SetRequestHeader("Authorization", $"Key {apiKey}");
            await statusRequest.SendWebRequest();

            if (statusRequest.result != UnityWebRequest.Result.Success)
            {
                throw new TripoSRException($"Failed to check status: {statusRequest.error}");
            }

            var statusResponse = JsonUtility.FromJson<StatusResponse>(statusRequest.downloadHandler.text);
            isCompleted = statusResponse.status == "COMPLETED";
            responseUrl = statusResponse.response_url;

        } while (!isCompleted);

        return responseUrl;
    }

    private async Task<string> FetchResult(string resultUrl)
    {
        using var resultRequest = UnityWebRequest.Get(resultUrl);
        resultRequest.SetRequestHeader("Authorization", $"Key {apiKey}");
        await resultRequest.SendWebRequest();

        if (resultRequest.result != UnityWebRequest.Result.Success)
        {
            throw new TripoSRException($"Failed to fetch result: {resultRequest.error}");
        }

        var resultResponse = JsonUtility.FromJson<TripoSRResponse>(resultRequest.downloadHandler.text);
        return resultResponse.model_mesh.url;
    }

    // Internal response classes
    [System.Serializable]
    private class QueueResponse
    {
        public string request_id;
        public string response_url;
        public string status_url;
    }

    [System.Serializable]
    private class StatusResponse
    {
        public string status;
        public string response_url;
    }
}
}

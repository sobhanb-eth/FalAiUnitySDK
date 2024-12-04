// Runtime/Scripts/Utils/ImageUploader.cs

using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace TripoSR.SDK
{
    public class ImageUploader
    {
        private readonly string apiKey;
        private const string UPLOAD_URL = "https://api.fal.ai/v1/upload";  // Updated URL path

        public ImageUploader(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<string> UploadImage(string imagePath)
        {
            byte[] imageData = System.IO.File.ReadAllBytes(imagePath);

            var form = new WWWForm();
            form.AddBinaryData("file", imageData, "image.png", "image/png");

            using var request = UnityWebRequest.Post(UPLOAD_URL, form);
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            request.SetRequestHeader("Content-Type", "multipart/form-data");
        
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new TripoSRException($"Image upload failed: {request.error}");
            }

            var response = JsonUtility.FromJson<UploadResponse>(request.downloadHandler.text);
            return response.url;
        }
    
        [Serializable]
        private class UploadResponse
        {
            public string url;
        }
    }
}


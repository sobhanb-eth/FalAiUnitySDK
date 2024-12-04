// Runtime/Scripts/Core/TripoSRManager.cs
using UnityEngine;
using System.Threading.Tasks;
using GLTFast;
using System;

namespace TripoSR.SDK
{
    public class TripoSRManager : MonoBehaviour
{
    private static TripoSRManager instance;
    private TripoSRConfig config;
    private TripoSRApi api;

    public static TripoSRManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("TripoSRManager");
                instance = go.AddComponent<TripoSRManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize(TripoSRConfig config)
    {
        this.config = config;
        this.api = new TripoSRApi(config);
    }

    public async Task<GameObject> ProcessImageUrl(
        string imageUrl,
        Transform parent = null,
        bool removeBackground = true,
        float foregroundRatio = 0.9f,
        int mcResolution = 256)
    {
        try
        {
            // Create the request object
            var request = new TripoSRRequest
            {
                image_url = imageUrl,
                output_format = "glb",
                do_remove_background = removeBackground,
                foreground_ratio = foregroundRatio,
                mc_resolution = mcResolution
            };

            // Submit to API and fetch model URL
            string modelUrl = await api.SubmitRequest(request);

            // Load GLB model
            return await LoadModel(modelUrl, parent);
        }
        catch (Exception e)
        {
            throw new TripoSRException($"Failed to process image URL: {e.Message}");
        }
    }

    private async Task<GameObject> LoadModel(string modelUrl, Transform parent)
    {
        var gltf = new GltfImport();

        // Load GLB model
        bool success = await gltf.Load(modelUrl);
        if (!success)
        {
            throw new TripoSRException("Failed to load GLB model");
        }

        var gameObject = new GameObject("TripoSRModel");
        if (parent != null)
        {
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
        }

        success = await gltf.InstantiateMainSceneAsync(gameObject.transform);
        if (!success)
        {
            Destroy(gameObject);
            throw new TripoSRException("Failed to instantiate GLB model");
        }

        return gameObject;
    }
}

}

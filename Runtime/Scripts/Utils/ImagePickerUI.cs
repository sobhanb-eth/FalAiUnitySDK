// Scripts/Utils/ImagePickerUI.cs
using UnityEngine;
using UnityEngine.UI;
using System;

public class ImagePickerUI : MonoBehaviour
{
    [SerializeField] private InputField imageUrlInputField;
    [SerializeField] private Button processButton;
    [SerializeField] private Transform modelParent;
    [SerializeField] private GameObject loadingIndicator;
    [SerializeField] private Text statusText;

    private void Start()
    {
        processButton.onClick.AddListener(OnProcessImageClick);
        loadingIndicator.SetActive(false);
    }

    private async void OnProcessImageClick()
    {
        string imageUrl = imageUrlInputField.text;
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            statusText.text = "Please enter a valid image URL.";
            return;
        }

        try
        {
            loadingIndicator.SetActive(true);
            statusText.text = "Processing image...";

            var model = await TripoSRManager.Instance.ProcessImageUrl(imageUrl, modelParent);
            statusText.text = "Model loaded successfully!";
        }
        catch (Exception e)
        {
            statusText.text = $"Error: {e.Message}";
        }
        finally
        {
            loadingIndicator.SetActive(false);
        }
    }
}
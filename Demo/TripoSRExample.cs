using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The TripoSRExample class serves as a basic template for a Unity MonoBehaviour script.
/// This class can be used as a starting point for creating custom behaviors and functionality
/// within a Unity project. It currently does not include any properties, methods, or logic,
/// and is intended to be expanded upon by game developers to meet specific needs of their
/// games or applications.
/// </summary>
public class TripoSRExample : MonoBehaviour
{
    /// <summary>
    /// Represents the API key required to authenticate requests to the Fal.ai service.
    /// </summary>
    /// <remarks>
    /// The API key is used during the initialization of the TripoSRManager to enable communication
    /// with the external Fal.ai service. This key should be kept confidential and not exposed
    /// publicly to prevent unauthorized access to the service.
    /// </remarks>
    /// <seealso cref="TripoSRManager"/>
    /// <seealso cref="TripoSRConfig"/>
    [Header("Fal.ai API Key")]
    [SerializeField] private string apiKey = "";

    /// <summary>
    /// Represents the input field for entering image URLs in the user interface.
    /// </summary>
    /// <remarks>
    /// This UI element is used to capture the image URL from the user, which will
    /// then be processed by the TripoSR application. The entered URL should point
    /// to an image resource accessible via the web.
    /// </remarks>
    [Header("UI Elements")]
    [SerializeField] private InputField imageUrlInputField; // Input field for image URL

    /// <summary>
    /// The button that triggers the processing of the image URL when clicked.
    /// </summary>
    [SerializeField] private Button processButton; // Button to trigger the process

    /// <summary>
    /// Represents the UI text component used to display status messages to the user.
    /// </summary>
    [SerializeField] private Text statusText; // Text UI to display status messages

    /// <summary>
    /// Represents the UI panel that serves as a loading indicator.
    /// It is used to indicate to the user that a process is currently underway,
    /// typically by becoming visible during time-consuming operations.
    /// </summary>
    [SerializeField] private GameObject loadingPanel; // Loading indicator

    /// <summary>
    /// The <c>modelContainer</c> variable acts as the parent container for the 3D model
    /// that is generated and loaded by the <see cref="TripoSRManager"/>.
    /// It represents a Transform in the Unity scene where the resulting model will be attached.
    /// </summary>
    [SerializeField] private Transform modelContainer; // Parent object for the 3D model

    // Exposed settings for Inspector
    /// <summary>
    /// Specifies whether the background should be removed from the input image when processed.
    /// </summary>
    /// <remarks>
    /// This variable is used as a setting for the model generation, allowing the user to choose
    /// if the background is to be removed or retained when creating the 3D model from an image.
    /// Default value is true, indicating that the background will be removed by default.
    /// </remarks>
    [Header("Model Generation Settings")]
    [SerializeField] private bool removeBackground = true; // Whether to remove the background from the input image. Default value: true

    /// <summary>
    /// The ratio of the foreground image to the original image, represented as a float value.
    /// This value affects the scaling of the foreground when the background is removed from
    /// an input image during model processing. It is adjustable in the Inspector using a custom
    /// slider attribute that allows a precision of one decimal place within the range of 0.5 to 1.
    /// The default is set to 0.9, which means the foreground will be scaled to 90% of the original image size.
    /// </summary>
    [SerializeField, OneDecimalSlider(0.5f, 1f)] private float foregroundRatio = 0.9f; // Ratio of the foreground image to the original image. Default value: 0.9

    /// <summary>
    /// Represents the resolution of the marching cubes algorithm used in 3D model generation.
    /// This variable affects the level of detail in the generated model.
    /// It is user-configurable through the Inspector within a specified range.
    /// </summary>
    /// <remarks>
    /// Recommended values are between 32 and 512, with the default being 256.
    /// Values above 512 may lead to performance issues.
    /// </remarks>
    [SerializeField, Range(32, 1024)] private int mcResolution = 256; // Resolution of the marching cubes. Above 512 is not recommended. Default value: 256

    /// <summary>
    /// Unity Awake method called when the script instance is being loaded.
    /// This method initializes the TripoSRManager with an API key, sets up UI elements,
    /// and assigns event handlers for user interaction. It ensures the loading panel
    /// is hidden initially and sets a default status message for the user.
    /// </summary>
    private void Awake()
    {
        // Initialize TripoSRManager with the API key
        TripoSRManager.Instance.Initialize(new TripoSRConfig
        {
            ApiKey = apiKey,
        });

        if (processButton != null)
        {
            // Assign button click event to process image URL
            processButton.onClick.AddListener(OnProcessImageClick);
        }

        if (loadingPanel != null)
        {
            // Ensure the loading panel is initially hidden
            loadingPanel.SetActive(false);
        }

        if (statusText != null)
        {
            // Set the default status message
            statusText.text = "Enter an image URL and press Process.";
        }
    }

    /// Handles the click event for the process button to process the provided image URL.
    /// This method retrieves the image URL from the input field and initiates the image
    /// processing procedure using the TripoSRManager. It updates the UI to reflect the
    /// current processing status, including displaying a loading indicator and status messages.
    /// Upon success, it will update the status text to indicate that the model has loaded successfully.
    /// If an error occurs during processing, it catches the exception and updates the status text
    /// with the error message.
    /// Prerequisites:
    /// - The API key should be set for the TripoSRManager to function.
    /// - The UI should contain properly linked elements: imageUrlInputField, processButton,
    /// statusText, loadingPanel, and modelContainer.
    /// Exceptions:
    /// - Handles exceptions that may occur during the processing of the image URL
    /// and logs them as error messages.
    private async void OnProcessImageClick()
    {
        string imageUrl = imageUrlInputField?.text;

        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            statusText.text = "Please enter a valid image URL.";
            return;
        }

        try
        {
            // Show the loading panel and update the status text
            loadingPanel?.SetActive(true);
            statusText.text = "Processing image...";

            // Process the image URL with Inspector-defined settings
            var model = await TripoSRManager.Instance.ProcessImageUrl(
                imageUrl,
                modelContainer,
                removeBackground,
                foregroundRatio,
                mcResolution
            );

            // Update the status text on success
            statusText.text = "Model loaded successfully!";
        }
        catch (System.Exception e)
        {
            // Handle errors and update the status text
            statusText.text = $"Error: {e.Message}";
            Debug.LogError(e);
        }
        finally
        {
            // Hide the loading panel after processing
            loadingPanel?.SetActive(false);
        }
    }
}

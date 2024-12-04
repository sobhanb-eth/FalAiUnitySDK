# FAL.ai Unity SDK

A Unity SDK for converting images to 3D models using the Fal.ai TripoSR service.
Features
Convert images to 3D models using URLs
Asynchronous processing with status updates
GLB model loading and scene integration
Configurable model generation settings
User-friendly UI components

## Installation

Add the package to your Unity project via Package Manager
Install required dependencies:
com.unity.cloud.gltfast (6.9.0 or higher)
com.unity.nuget.newtonsoft-json (3.0.2 or higher)

## Setup

Create a new GameObject in your scene
Add the following UI components:
text
Canvas

````
├── Image URL Input Field
├── Process Button
├── Status Text
└── Loading Panel
````


Create a Model Container GameObject to hold the generated 3D models
Initialize the TripoSR manager with your API key:
csharp
TripoSRManager.Instance.Initialize(new TripoSRConfig
{
ApiKey = "YOUR_FAL_AI_KEY"
});

## Usage

Basic Implementation

```csharp
// Process an image URL
await TripoSRManager.Instance.ProcessImageUrl(
imageUrl: "https://example.com/image.jpg",
parent: modelContainer,
removeBackground: true,
foregroundRatio: 0.9f,
mcResolution: 256
);
```
Configuration Options
```text
removeBackground: Remove image background during processing
foregroundRatio: Scale of foreground relative to original image (0.5-1.0)
mcResolution: Marching cubes resolution for model detail (32-1024)
```

API Structure

`TripoSRManager`: Singleton manager handling initialization and processing

`TripoSRApi`: Handles API communication and request queuing

`ImagePickerUI`: UI component for image URL input and processing

`GLTFast Integration`: Handles 3D model loading and scene instantiation

## Example Scene Setup

Create a new scene with the following hierarchy:

```text
Scene
├── Main Camera
├── Directional Light
├── Model Container
└── Canvas
├── URL Input Field
├── Process Button
├── Status Text
└── Loading Panel
```
Add the ImagePickerUI component to the Canvas and configure references:

```csharp
[SerializeField] private InputField imageUrlInputField;
[SerializeField] private Button processButton;
[SerializeField] private Transform modelParent;
[SerializeField] private GameObject loadingIndicator;
[SerializeField] private Text statusText;
```

## Error Handling

The system includes comprehensive error handling through the TripoSRException class. All operations are wrapped in try-catch blocks with appropriate error messages displayed in the UI.
API Response Format
The system handles the following response structure:

```csharp
public class TripoSRResponse
{
public ModelMesh model_mesh;
public Timings timings;
}
```

## Sample Scene

Import the sample scene from the Samples section in unity package manager
## Notes

Recommended
* mcResolution range: 32-512 (higher values may impact performance)
* Default foregroundRatio: 0.9

API requests are queued and processed asynchronously
Models are automatically positioned at the parent transform's origin

## Requirements

* Unity 2020.3 or higher
* .NET 4.x or higher
* Active Fal.ai API key
* Internet connection for API communication
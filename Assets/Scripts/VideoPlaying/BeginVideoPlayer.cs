using UnityEngine;
using UnityEngine.Video;

public class BeginVideoPlayer : MonoBehaviour
{
    public Material objectMaterial; // Assign your object's material
    public VideoPlayer videoPlayer; // Assign your VideoPlayer component
    public RenderTexture renderTexture; // Default render texture (if any)
    public int materialIndex = 0; 
        
    void Awake()
    {
        // Check if the necessary components are assigned
        if (objectMaterial == null || videoPlayer == null)
        {
            Debug.LogError("Missing required components: objectMaterial or videoPlayer.");
            return;
        }

        // Create a unique RenderTexture for this object
        RenderTexture uniqueTexture = new RenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth);
    
        // Create a unique material instance by duplicating the original material
        Material uniqueMaterial = new Material(objectMaterial);

        // Assign the unique RenderTexture to the unique material
        uniqueMaterial.SetTexture("_Base", uniqueTexture);

        // Apply the unique material to the renderer of the object
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && renderer.materials.Length > 1)
        {
            // Assign the material to the second slot (index 1)
            Material[] materials = renderer.materials;
            materials[materialIndex] = uniqueMaterial; // Replace the material at index 1
            renderer.materials = materials; // Reassign the materials array
        }
        else
        {
            Debug.LogError("Renderer or materials not found.");
            return;
        }

        // Assign the RenderTexture to the VideoPlayer
        videoPlayer.targetTexture = uniqueTexture;

        // Ensure the VideoPlayer is set up properly (e.g., a valid video clip is assigned)
        if (videoPlayer.clip == null)
        {
            Debug.LogError("No video clip assigned to the VideoPlayer.");
            return;
        }
        
        videoPlayer.frame = -10;
        videoPlayer.Pause();
    }

    public void HandlePlay(bool play)
    {
        if (play)
        {
            videoPlayer.frame = Random.Range(0, 1000);
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.frame = -10;
            videoPlayer.Pause();
        }

    }
}
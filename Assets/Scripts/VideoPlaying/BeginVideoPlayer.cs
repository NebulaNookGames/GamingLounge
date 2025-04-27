using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class BeginVideoPlayer : MonoBehaviour
{
    public VideoClip linuxClip;
    public VideoClip windowsClip; 
    
    public Material objectMaterial; // Assign your object's material
    public Material logoMaterial;
    public Material gameplayImageMaterial; 
    public VideoPlayer videoPlayer; // Assign your VideoPlayer component
    public RenderTexture renderTexture; // Default render texture (if any)
    public Material uniqueMaterial; 
    public int materialIndex = 0;
    private bool currentPlay;
    public int minStartFrame = 400;
    public int maxStartFrame = 600;
    private bool initialPlay = true;


    private void Awake()
    {
        videoPlayer.clip = null; 
    }

    public void HandlePlay(bool play)
    {
#if !UNITY_SWITCH
        currentPlay = play; 
        videoPlayer.Prepare();
        Invoke(nameof(StartOrPause), .2f);
#endif 
#if UNITY_SWITCH
        if (play)
            uniqueMaterial = gameplayImageMaterial;
        else
            uniqueMaterial = logoMaterial;
        
        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = renderer.materials;
        materials[materialIndex] = uniqueMaterial;
        renderer.materials = materials;
#endif 
    }

    void StartOrPause()
    {
        if (initialPlay)
        {
            initialPlay = false;
            InitializeVideo();
        }
        
        if (currentPlay)
        {
            if (Application.platform == RuntimePlatform.LinuxPlayer)
                videoPlayer.clip = linuxClip;
            else
                videoPlayer.clip = windowsClip;
            
            videoPlayer.frame = Random.Range(minStartFrame, maxStartFrame); 
            Renderer renderer = GetComponent<Renderer>();
            Material[] materials = renderer.materials;
            materials[materialIndex] = uniqueMaterial;
            renderer.materials = materials; 
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.clip = null; 
            Renderer renderer = GetComponent<Renderer>();
            Material[] materials = renderer.materials;
            materials[materialIndex] = logoMaterial;
            renderer.materials = materials; 
        }
    }

    void InitializeVideo()
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
        uniqueMaterial = new Material(objectMaterial);

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
            return;
        }

        // Assign the RenderTexture to the VideoPlayer
        videoPlayer.targetTexture = uniqueTexture;
    }
}
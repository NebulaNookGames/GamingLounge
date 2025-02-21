using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    // Steam Deck's native resolution
    private int deckWidth = 1280;
    private int deckHeight = 800;

    void Start()
    {
        AdjustResolution();
    }

    void AdjustResolution()
    {
        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;

        // Only change resolution if needed
        if (currentWidth != deckWidth || currentHeight != deckHeight)
        {
            Debug.Log($"Current Resolution: {currentWidth}x{currentHeight}");

            // Set resolution only if it's Steam Deck's dimensions
            if (currentWidth == deckWidth && currentHeight == deckHeight)
            {
                Screen.SetResolution(deckWidth, deckHeight, true);
                Debug.Log("Resolution adjusted for Steam Deck.");
            }
            else
            {
                Debug.Log("Running on a different device. Resolution unchanged.");
            }
        }
    }
}
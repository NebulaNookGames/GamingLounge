// Written by Kevin Catlett.

using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Scales the intensity of a 2D light up and down to simulate flickering. 
/// </summary>
public class LightFlicker : MonoBehaviour
{
    [Tooltip("The light to control.")]
    [SerializeField] Light myLight;

    [Tooltip("The minimum intensity of the light.")]
    [Range(0,5)]
    [SerializeField] float minIntensity; 

    [Tooltip("The maximum intensity of the light.")]
    [Range(0,10)]
    [SerializeField] float maxIntensity;

    [Tooltip("How fast the light should change intensity.")]
    [SerializeField] float dimSpeed;

    bool dim; // Should the intensity be decreasing or increasing.
    
    /// <summary>
    /// Gets a reference to the light2D on this gameObject.
    /// </summary>
    void Start()
    {
        if (!myLight && GetComponent<Light>())
        {
            myLight = GetComponent<Light>();
        }
    }

    /// <summary>
    /// Calls the Flicker method.
    /// </summary>
    void Update()
    {
        Flicker();
    }

    /// <summary>
    /// Scales the intensity of a 2D light up and down to simulate flickering. 
    /// </summary>
    void Flicker()
    {
        if (dim)
        {
            myLight.intensity -= dimSpeed * Time.deltaTime;
            if (myLight.intensity <= minIntensity)
            {
                dim = false;
            }
        }

        else if (!dim)
        {
            myLight.intensity += dimSpeed * Time.deltaTime;
            if (myLight.intensity >= maxIntensity)
            {
                dim = true;
            }
        }
    }
}

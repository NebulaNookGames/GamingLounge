using UnityEngine;

/// <summary>
/// Scales the intensity of a 2D light up and down to simulate flickering. 
/// </summary>
public class LightFlicker : MonoBehaviour
{
    #region Serialized Fields

    [Tooltip("The light to control.")]
    [SerializeField] Light myLight;

    [Tooltip("The minimum intensity of the light.")]
    [Range(0, 5)]
    [SerializeField] float minIntensity;

    [Tooltip("The maximum intensity of the light.")]
    [Range(0, 10)]
    [SerializeField] float maxIntensity;

    [Tooltip("How fast the light should change intensity.")]
    [SerializeField] float dimSpeed;

    #endregion

    #region Private Fields

    bool dim; // Should the intensity be decreasing or increasing.

    #endregion

    #region Unity Methods

    /// <summary>
    /// Gets a reference to the light on this GameObject if not already assigned.
    /// </summary>
    void Start()
    {
        if (!myLight && GetComponent<Light>())
        {
            myLight = GetComponent<Light>();
        }
    }

    /// <summary>
    /// Calls the Flicker method to adjust light intensity.
    /// </summary>
    void Update()
    {
        Flicker();
    }

    #endregion

    #region Flicker Logic

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
        else
        {
            myLight.intensity += dimSpeed * Time.deltaTime;
            if (myLight.intensity >= maxIntensity)
            {
                dim = true;
            }
        }
    }

    #endregion
}
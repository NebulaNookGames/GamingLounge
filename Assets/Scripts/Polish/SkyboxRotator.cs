using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    #region Serialized Fields

    // Rotation speed in degrees per second
    public float rotationSpeed = 10f;

    #endregion

    #region Unity Methods

    void Update()
    {
        // Calculate the new rotation angle
        float rotation = Time.time * rotationSpeed;

        // Apply the rotation to the skybox's "_Rotation" property
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }

    #endregion
}
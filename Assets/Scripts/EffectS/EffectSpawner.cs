using System;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    #region Serialized Fields

    // The effect GameObject that will be instantiated when spawning
    [SerializeField] public GameObject effect;

    #endregion

    #region Unity Methods

    // Called when the script is enabled (e.g., when the GameObject becomes active)
    private void OnEnable()
    {
        // Instantiate the effect at the GameObject's position and rotation when it is enabled
        Instantiate(effect, transform.position, transform.rotation);
    }

    #endregion

    #region Public Methods

    // Method to manually spawn the effect at the GameObject's position and rotation
    public void SpawnEffect()
    {
        // Instantiate the effect at the GameObject's position and rotation
        Instantiate(effect, transform.position, transform.rotation);
    }

    #endregion
}
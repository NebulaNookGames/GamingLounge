using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class EyeBlink : MonoBehaviour
{
    #region Serialized Fields

    public Renderer faceRenderer;                // The renderer for the face object
    public int eyeIndex;                         // Index of the eye material in the materials array
    public int skinIndex;                        // Index of the skin material to swap with during blink
    public Material normalMaterial;              // The normal material to be used for the eye
    public float blinkInterval = 5f;             // Base interval for blinking
    public float blinkRandomRange = 1.5f;        // Random variation in the blink interval (+/-)
    public float blinkDuration = 0.1f;           // Duration of the blink

    #endregion

    #region Unity Methods
    
    private void OnEnable()
    {
        StartCoroutine(BlinkRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(BlinkRoutine());
    }

    #endregion

    #region Coroutine Methods

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            // Wait for a random time before blinking
            float nextBlinkTime = blinkInterval + Random.Range(-blinkRandomRange, blinkRandomRange);
            yield return new WaitForSeconds(nextBlinkTime);

            // Swap to blinking material
            Material[] mats = faceRenderer.materials;
            mats[eyeIndex] = mats[skinIndex];
            faceRenderer.materials = mats;
            
            yield return new WaitForSeconds(blinkDuration);

            // Swap back to normal material
            mats = faceRenderer.materials;
            mats[eyeIndex] = normalMaterial;
            faceRenderer.materials = mats;
        }
    }

    #endregion
}
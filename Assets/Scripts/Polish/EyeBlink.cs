using UnityEngine;
using System.Collections;

public class EyeBlink : MonoBehaviour
{
    public Renderer faceRenderer;
    public int eyeIndex;
    public int skinIndex;
    public Material normalMaterial;
    public float blinkInterval = 5f; // Base interval
    public float blinkRandomRange = 1.5f; // Max random variation (+/-)
    public float blinkDuration = 0.1f;

    void Start()
    {
        StartCoroutine(BlinkRoutine());
    }

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
}
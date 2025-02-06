using System;
using UnityEngine;
using EditorAttributes;
using System.Collections.Generic;

public class CharacterCustomization : MonoBehaviour
{
    public enum CharacterPart
    {
        Shoe,
        Hand,
        Suit,
        SuitDetail,
        Hair,
        Skin
    };
    
    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;

    private Material originalShoesMaterial;
    private Material originalHandMaterial;
    private Material originalSuitMaterial;
    private Material originalSuitDetailMaterial;
    private Material originalHairMaterial; 
    private Material originalSkinMaterial;
    
    [SerializeField] private Material[] shoeMaterials;
    [SerializeField] private Material[] handMaterials;
    [SerializeField] private Material[] suitMaterials;
    [SerializeField] private Material[] suitDetailMaterials;
    [SerializeField] private Material[] hairMaterials;
    [SerializeField] private Material[] skinMaterials;
    
    private int currentShoeIndex = 0;
    private int currentHandIndex = 0;
    private int currentSuitIndex = 0;
    private int currentSuitDetailIndex = 0;
    private int currentHairIndex = 0;
    private int currentSkinIndex = 0;
    
    private void Awake()
    {
        originalShoesMaterial = playerMeshRenderer.sharedMaterials[0];
        originalHandMaterial = playerMeshRenderer.sharedMaterials[1];
        originalSuitMaterial = playerMeshRenderer.sharedMaterials[2];
        originalSuitDetailMaterial = playerMeshRenderer.sharedMaterials[3];
        originalHairMaterial = playerMeshRenderer.sharedMaterials[4];
        originalSkinMaterial = playerMeshRenderer.sharedMaterials[5];
    }
    
    public void CustomizeShoeNext() => CustomizeToNext(CharacterPart.Shoe);
    public void CustomizeHandNext() => CustomizeToNext(CharacterPart.Hand);
    public void CustomizeSuitNext() => CustomizeToNext(CharacterPart.Suit);
    public void CustomizeSuitDetailNext() => CustomizeToNext(CharacterPart.SuitDetail);
    public void CustomizeHairNext() => CustomizeToNext(CharacterPart.Hair);
    
    public void CustomizeSkinNext() => CustomizeToNext(CharacterPart.Skin);
    
    public void CustomizeShoePrevious() => CustomizeToPrevious(CharacterPart.Shoe);
    public void CustomizeHandPrevious() => CustomizeToPrevious(CharacterPart.Hand);
    public void CustomizeSuitPrevious() => CustomizeToPrevious(CharacterPart.Suit);
    public void CustomizeSuitDetailPrevious() => CustomizeToPrevious(CharacterPart.SuitDetail);
    public void CustomizeHairPrevious() => CustomizeToPrevious(CharacterPart.Hair);
    
    public void CustomizeSkinPrevious() => CustomizeToPrevious(CharacterPart.Skin);

    [Button]
public void CustomizeToNext(CharacterPart characterPart)
{
    Material[] materials = playerMeshRenderer.materials; // Get a copy of the current materials array

    switch (characterPart)
    {
        case CharacterPart.Shoe:
            currentShoeIndex = (currentShoeIndex + 1) % shoeMaterials.Length;
            materials[0] = shoeMaterials[currentShoeIndex];
            break;

        case CharacterPart.Hand:
            currentHandIndex = (currentHandIndex + 1) % handMaterials.Length;
            materials[1] = handMaterials[currentHandIndex];
            break;

        case CharacterPart.Suit:
            currentSuitIndex = (currentSuitIndex + 1) % suitMaterials.Length;
            materials[2] = suitMaterials[currentSuitIndex];
            break;

        case CharacterPart.SuitDetail:
            currentSuitDetailIndex = (currentSuitDetailIndex + 1) % suitDetailMaterials.Length;
            materials[3] = suitDetailMaterials[currentSuitDetailIndex];
            break;

        case CharacterPart.Hair:
            currentHairIndex = (currentHairIndex + 1) % hairMaterials.Length;
            materials[4] = hairMaterials[currentHairIndex];
            break;
        
        case CharacterPart.Skin:
            currentSkinIndex = (currentSkinIndex + 1) % skinMaterials.Length;
            materials[5] = skinMaterials[currentSkinIndex];
            break;
    }

    playerMeshRenderer.materials = materials; // Assign the modified materials array back to the renderer
}

[Button]
public void CustomizeToPrevious(CharacterPart characterPart)
{
    Material[] materials = playerMeshRenderer.materials; // Get a copy of the current materials array

    switch (characterPart)
    {
        case CharacterPart.Shoe:
            currentShoeIndex = (currentShoeIndex - 1 + shoeMaterials.Length) % shoeMaterials.Length;
            materials[0] = shoeMaterials[currentShoeIndex];
            break;

        case CharacterPart.Hand:
            currentHandIndex = (currentHandIndex - 1 + handMaterials.Length) % handMaterials.Length;
            materials[1] = handMaterials[currentHandIndex];
            break;

        case CharacterPart.Suit:
            currentSuitIndex = (currentSuitIndex - 1 + suitMaterials.Length) % suitMaterials.Length;
            materials[2] = suitMaterials[currentSuitIndex];
            break;

        case CharacterPart.SuitDetail:
            currentSuitDetailIndex = (currentSuitDetailIndex - 1 + suitDetailMaterials.Length) % suitDetailMaterials.Length;
            materials[3] = suitDetailMaterials[currentSuitDetailIndex];
            break;

        case CharacterPart.Hair:
            currentHairIndex = (currentHairIndex - 1 + hairMaterials.Length) % hairMaterials.Length;
            materials[4] = hairMaterials[currentHairIndex];
            break;
        
        case CharacterPart.Skin:
            currentSkinIndex = (currentSkinIndex - 1 + skinMaterials.Length) % skinMaterials.Length;
            materials[5] = skinMaterials[currentSkinIndex];
            break;
    }

    playerMeshRenderer.materials = materials; // Assign the modified materials array back to the renderer
}

[Button]
public void Reset()
{
    Material[] materials = playerMeshRenderer.materials; // Get the current materials array

    // Reset only the customized materials while keeping the rest unchanged
    materials[0] = originalShoesMaterial;
    materials[1] = originalHandMaterial;
    materials[2] = originalSuitMaterial;
    materials[3] = originalSuitDetailMaterial;
    materials[4] = originalHairMaterial;
    materials[5] = originalSkinMaterial;

    // Assign the modified materials array back to the renderer
    playerMeshRenderer.materials = materials;

    // Reset indexes
    currentShoeIndex = 0;
    currentHandIndex = 0;
    currentSuitIndex = 0;
    currentSuitDetailIndex = 0;
    currentHairIndex = 0;
    currentSkinIndex = 0;
}

}

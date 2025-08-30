using UnityEngine;
using System.Collections.Generic; 
using TMPro; 

public class VisitorInfo : MonoBehaviour
{
    [SerializeField] List<string> alienNames = new List<string>();
    public List<string> AlienNames { get { return alienNames; } }

    public TextMeshProUGUI text; 
    
    void Start()
    {
        LoadNames();
        int randomInt = Random.Range(0, alienNames.Count);
        text.text = alienNames[randomInt];
    }

    void LoadNames()
    {
        TextAsset nameFile = Resources.Load<TextAsset>("AlienNames"); // No need to include .txt
        if (nameFile != null)
        {
            string[] names = nameFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            alienNames.AddRange(names);

            Debug.Log($"Loaded {alienNames.Count} alien names.");
        }
        else
        {
            Debug.LogError("AlienNames.txt not found in Resources folder.");
        }
    }
}

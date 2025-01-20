using UnityEngine;

public class HelperPanelHandler : MonoBehaviour
{
    public float destroyAfter;

    public GameObject helperCanvas; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        Invoke(nameof(Perform), 5f);
    }
    
    void Perform()
    {
        if (!SaveAndLoad.instance.saveDataLoaded)
        {
            helperCanvas.SetActive(true);
            Destroy(gameObject, destroyAfter);
        }
        else
            Destroy(gameObject);
    }
}

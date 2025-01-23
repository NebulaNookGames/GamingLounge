using UnityEngine;
using UnityEngine.Events;

public class ActivatePlacedObject : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;
    private AddMoneyOnDestroy addMoneyOnDestroy;

    public UnityEvent OnActivatePlacedObject;
    public UnityEvent OnActivatePlacedObjectCallAlways; 
    private void OnEnable()
    {
        foreach (Collider col in colliders)
        { 
            col.enabled = false;
        }
        
        addMoneyOnDestroy = GetComponent<AddMoneyOnDestroy>();
    }

    public void Enable(bool callEvent)
    {
        foreach (Collider col in colliders)
        { 
            col.enabled = true;
        }
        
        addMoneyOnDestroy.enabled = true; 
        
        if(callEvent)
            OnActivatePlacedObject?.Invoke();
        
        OnActivatePlacedObjectCallAlways?.Invoke();
    }
    
}
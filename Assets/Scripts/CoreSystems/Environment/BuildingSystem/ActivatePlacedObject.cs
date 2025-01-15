using UnityEngine;
using UnityEngine.Events;

public class ActivatePlacedObject : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;
    private AddMoneyOnDestroy addMoneyOnDestroy;

    public UnityEvent OnActivatePlacedObject; 
    
    private void OnEnable()
    {
        foreach (Collider col in colliders)
        { 
            col.enabled = false;
        }
        
        addMoneyOnDestroy = GetComponent<AddMoneyOnDestroy>();
    }

    public void Enable()
    {
        foreach (Collider col in colliders)
        { 
            col.enabled = true;
        }
        
        addMoneyOnDestroy.enabled = true; 
        OnActivatePlacedObject?.Invoke();
    }
}
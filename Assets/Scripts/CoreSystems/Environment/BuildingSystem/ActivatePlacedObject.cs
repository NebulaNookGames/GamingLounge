using UnityEngine;
using UnityEngine.Events;

public class ActivatePlacedObject : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;
    private AddMoneyOnDestroy addMoneyOnDestroy;

    public UnityEvent OnActivatePlacedObject;
    public UnityEvent OnActivatePlacedObjectCallAlways;
    public UnityEvent OnDeactivatePlaceObject; 
    
    private void OnEnable()
    {
        if (colliders.Length > 0 && colliders != null)
        {
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
        }

        addMoneyOnDestroy = GetComponent<AddMoneyOnDestroy>();
    }

    public void Enable(bool callEvent)
    {
        if (colliders.Length > 0 && colliders != null)
        {
            foreach (Collider col in colliders)
            {
                col.enabled = true;
            }
        }

        addMoneyOnDestroy.enabled = true; 
        
        if(callEvent)
            OnActivatePlacedObject?.Invoke();
        
        OnActivatePlacedObjectCallAlways?.Invoke();
    }

    public void Deactivate()
    {
        OnDeactivatePlaceObject?.Invoke();
    }
}
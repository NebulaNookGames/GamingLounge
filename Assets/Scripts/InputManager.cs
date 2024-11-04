using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;

    public event Action OnClicked, OnPlacementToggle, OnRotate;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            OnPlacementToggle?.Invoke();
        else if(Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
        else if (Input.GetKeyDown(KeyCode.R))
            OnRotate?.Invoke();
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private Vector3 lastPosition;

    [SerializeField] private LayerMask placementLayermask;

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;    
    }
}

using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class UserInterface : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    public GameObject hospital;
    GameObject focusObj;
    public GameObject newResourcePrefab;
    Vector3 goalPos;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(!Physics.Raycast(ray, out hit))
            {
                return;
            }

            goalPos = hit.point;
            focusObj = Instantiate(newResourcePrefab, goalPos, newResourcePrefab.transform.rotation);
        }

        else if (focusObj && Input.GetMouseButtonUp(0))
        {
            focusObj.transform.parent = hospital.transform;
            navMeshSurface.BuildNavMesh();

            World.Instance.GetQueue("toilets").AddResource(focusObj);
            World.Instance.GetWorld().ModifyState("FreeToilet", 1);
            focusObj = null;
        }

        else if(focusObj && Input.GetMouseButton(0))
        {
            RaycastHit hitMove;
            Ray rayMove = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(rayMove, out hitMove))
            {
                return;
            }

            goalPos = hitMove.point;
            focusObj.transform.position = goalPos;
        }

        if(focusObj && (Input.GetKeyDown(KeyCode.Less) || focusObj && Input.GetKeyDown(KeyCode.Comma)))
        {
            focusObj.transform.Rotate(0, 90, 0);
        }
        else if(focusObj && (Input.GetKeyDown(KeyCode.Greater) || focusObj && Input.GetKeyDown(KeyCode.Period)))
        {
            focusObj.transform.Rotate(0, -90, 0);
        }
    }
}

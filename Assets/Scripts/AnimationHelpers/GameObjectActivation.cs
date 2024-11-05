using UnityEngine;

public class GameObjectActivation : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjectsToActivate;

    public void Activate()
    {
        foreach (GameObject go in gameObjectsToActivate)
            go.SetActive(true);
    }

    public void Deactivate()
    {
        foreach (GameObject go in gameObjectsToActivate)
            go.SetActive(false);
    }
}

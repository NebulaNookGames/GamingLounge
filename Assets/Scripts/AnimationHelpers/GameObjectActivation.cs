using UnityEngine;

public class GameObjectActivation : MonoBehaviour
{
    [SerializeField] private GameObject gameObject;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

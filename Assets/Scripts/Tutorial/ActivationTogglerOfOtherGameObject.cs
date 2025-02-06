using System;
using UnityEngine;

public class ActivationTogglerOfOtherGameObject : MonoBehaviour
{
    public GameObject gameObjectToActivate;
    public float waitDuration;

    private void OnEnable()
    {
        Invoke(nameof(DisableThisAndActivateOther), waitDuration);
    }

    void DisableThisAndActivateOther()
    {
        gameObjectToActivate.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if(IsInvoking(nameof(DisableThisAndActivateOther)))
            CancelInvoke(nameof(DisableThisAndActivateOther));
    }
}

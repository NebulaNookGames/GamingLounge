using System;
using UnityEngine;

public class RemoveFromParentOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        transform.parent = null;
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    #region Singleton

    // Singleton instance of the InputReader class
    public static InputReader instance;

    #endregion

    #region Serialized Fields

    // The locomotion input action that reads the player's movement input
    public InputAction locomotionInputAction;

    #endregion

    #region Private Variables

    // Flag indicating if the "run" input action is pressed
    public bool runPressed;

    #endregion

    #region Unity Methods

    // Called when the script is initialized
    private void Awake()
    {
        // Initialize the singleton instance
        instance = this;
    }

    // Called when the script is enabled
    private void OnEnable()
    {
        // Enable the locomotion input action
        locomotionInputAction.Enable();
    }

    // Called when the script is disabled
    private void OnDisable()
    {
        // Disable the locomotion input action
        locomotionInputAction.Disable();
    }

    #endregion

    #region Input Methods

    // Returns the horizontal movement input (left/right)
    public float GetHorizontalAxis()
    {
        return locomotionInputAction.ReadValue<Vector2>().y;
    }

    // Returns the vertical movement input (forward/backward)
    public float GetVerticalAxis()
    {
        return locomotionInputAction.ReadValue<Vector2>().x;
    }

    #endregion
}
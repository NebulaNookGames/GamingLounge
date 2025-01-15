using System;
using UnityEngine;
using UnityEngine.InputSystem; 

/// <summary>
/// Pauses/Unpauses the game and opens the Character Selection.
/// </summary>
public class PauseHandler : MonoBehaviour
{
   [Header("GameObjects")]
   [Tooltip("The gameObject that the pause Canvas is on.")]
   [SerializeField] GameObject pauseMenu;

   [SerializeField] private GameObject gameUI; 
   
   [Header("")]
   [Tooltip("How long until the game can be paused.")]
   [SerializeField] float pauseLockedTime;

   public InputActionProperty pauseAction; 
   
   // Is the game currently paused.
   bool isPaused;

   public bool pausedByOtherSystem;
   public bool IsPaused
   {
      get { return isPaused;}
      set { isPaused = value; }
   }
   
   #region Methods

   private void OnEnable()
   {
      pauseAction.action.performed += Pause; 
   }

   private void OnDisable()
   {
      pauseAction.action.performed -= Pause;
   }

   void Pause(InputAction.CallbackContext context)
   {
      if (pausedByOtherSystem) return; 
      
      HandlePause(); 
   }
   
   /// <summary>
   /// Pauses or resumes the game depending on the isPaused boolean.
   /// </summary>
   public void HandlePause()
   {
      isPaused = !isPaused;

      if (isPaused)
      {
         GameInput.Instance.SetMouseVisibility(true);
         Time.timeScale = 0;
         pauseMenu.SetActive(true);
         gameUI.SetActive(false);
      }
      else
      {
         GameInput.Instance.SetMouseVisibility(false);
         Time.timeScale = 1;
         pauseMenu.SetActive(false);
         gameUI.SetActive(true);
      }
   }
   #endregion
}

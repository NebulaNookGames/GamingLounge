using System.Collections;
using UnityEngine;

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
   
   // Is the game currently paused.
   bool isPaused;

   public bool pausedByOtherSystem;
   public bool IsPaused
   {
      get { return isPaused;}
      set { isPaused = value; }
   }
   
   #region Methods
   /// <summary>
   /// Hides the cursor and calls the UnlockPause coroutine.
   /// </summary>
   void Awake()
   {
      Cursor.visible = false;
   }

   /// <summary>
   /// Calls the HandlePause method if the conditions for pausing are met.
   /// </summary>
   void Update()
   {
      if(Input.GetKeyDown(KeyCode.Escape) && !pausedByOtherSystem 
         || Input.GetKeyDown(KeyCode.Joystick1Button7) && !pausedByOtherSystem)
      {
         HandlePause();
      }
   }
   
   /// <summary>
   /// Pauses or resumes the game depending on the isPaused boolean.
   /// </summary>
   public void HandlePause()
   {
      isPaused = !isPaused;

      if (isPaused)
      {
         Time.timeScale = 0;
         Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None;
         pauseMenu.SetActive(true);
         gameUI.SetActive(false);
      }
      else
      {
         Time.timeScale = 1;
         Cursor.visible = false;
         Cursor.lockState = CursorLockMode.Locked;
         pauseMenu.SetActive(false);
         gameUI.SetActive(true);
      }
   }
   #endregion
}

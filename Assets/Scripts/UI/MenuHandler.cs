using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Entails essential logic for loading scenes and quitting the application. 
/// </summary>
public class MenuHandler : MonoBehaviour
{
    [Tooltip("The animator that handles the transitiong.")] [SerializeField]
    Animator transitionAnimator;

    [Tooltip("The time until the transition ends and the scene turns to visible.")] [SerializeField]
    float transitionInTime;

    [Tooltip("The name of the trigger parameter inside of the transitionAnimator that starts the transition.")]
    [SerializeField]
    string transitionInTrigger;

    private GameInput gameInput; 
    
    #region Methods

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        if (GameObject.FindWithTag("TransitionAnimator"))
        {
            transitionAnimator = GameObject.FindWithTag("TransitionAnimator").GetComponent<Animator>();
            Invoke(nameof(StartTransition), transitionInTime);
        }
    }


    void StartTransition()
    {
        if(transitionAnimator)
            transitionAnimator.SetTrigger(transitionInTrigger);
    }
    
    /// <summary>
    /// Opens a scene with the index of the "sceneIndex" parameter. 
    /// </summary>
    /// <param name="sceneIndex"></param> The index of the scene which should be opened.
    public void OpenScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneCoroutine(sceneIndex));
    }

    IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        if(transitionAnimator)
            transitionAnimator.SetTrigger(transitionInTrigger);
        
        yield return new WaitForSecondsRealtime(transitionInTime);
        SceneManager.LoadScene(sceneIndex);
    }
    
    
    /// <summary>
    /// Quits to the desktop if called in build.
    /// Exits play mode if called in editor.
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    #endregion

    public void DeleteSave()
    {
        try
        {
            File.Delete(Application.persistentDataPath + "/saveFile.json");
        }
        catch
        {
            Debug.Log("Failed to delete save file");
        }
    }
}

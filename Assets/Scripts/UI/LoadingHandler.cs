using System;
using UnityEngine;
using UnityEngine.UI;
public class LoadingHandler : MonoBehaviour
{
    [Tooltip("The slider used for representing a load bar.")]
    [SerializeField] Slider loadingBar;
    
    [Tooltip("The time until the transition is ended and the game begins.")]
    [SerializeField] float loadingTime;
    
    [Tooltip("The amount in which the loading bar gets filled.")]
    [SerializeField] float loadingBarIncrementAmount;
    
    [Tooltip("The animation that begins and ends the transition.")]
    [SerializeField] Animator transitionAnimator;
    
    // True when the loading time has ended and the game should begin.
    bool loadingComplete;
    
    #region Methods
    /// <summary>
    /// Disables the transition animator so the transition does not occur when the scene
    /// first loads. 
    /// </summary>
    void Start()
    {
        transitionAnimator.enabled = false;
    }

    /// <summary>
    /// Calls the HandleLoadBar method.
    /// </summary>
    void Update()
    {
       HandleLoadBar();
    }

    /// <summary>
    /// Counts down until the loadingTime has reached zero.
    /// Afterwards deactivates the loading bar and enables the transitionAnimator, which will the start the scene.
    /// </summary>
    void HandleLoadBar()
    {
        if (!loadingComplete)
        {
            loadingTime -= Time.deltaTime;
            loadingBar.value += loadingBarIncrementAmount * Time.deltaTime;
            if (loadingTime <= 0)
            {
                loadingComplete = true;
                loadingBar.gameObject.SetActive((false));
                transitionAnimator.enabled = true;
            }
        }
    }
    #endregion
}

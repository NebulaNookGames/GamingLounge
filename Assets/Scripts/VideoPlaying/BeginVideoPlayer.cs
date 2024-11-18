using System;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Controls the behavior of a VideoPlayer component, starting playback on Awake.
/// </summary>
public class BeginVideoPlayer : MonoBehaviour
{
    /// <summary>
    /// Reference to the VideoPlayer component that will play the video.
    /// </summary>
    public VideoPlayer videoPlayer;

    /// <summary>
    /// Automatically starts video playback when the object is initialized.
    /// </summary>
    private void Awake()
    {
        videoPlayer.Play();
    }
}
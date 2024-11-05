using System;
using UnityEngine;
using UnityEngine.Video;

public class BeginVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer.Play();
    }
}

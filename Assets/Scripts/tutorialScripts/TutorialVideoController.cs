using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Video;

public class TutorialVideoController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField]
    private float videoStartTime = 0.5f;
    private GameObject outputTexture;
    private float videoFadeInTime = 0.5f;
    void Start(){
    }
    void OnEnable()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.time = videoStartTime;
        outputTexture = transform.GetChild(0).gameObject;
        outputTexture.SetActive(true);
        videoFadeInTime = GetComponentInParent<MoveAcrossTrailStep>().videoFadeInTime;
        print("invokign play video in " + videoFadeInTime + " seconds");
        print("go is active: " + gameObject.activeSelf);
        //videoPlayer.Play();

        StartCoroutine("corout");
    }

    private void OnDisable() {
        videoPlayer.Stop();
        outputTexture.SetActive(false);
    }

    private IEnumerator corout() {
        yield return new WaitForSeconds(videoFadeInTime);
        print("playing video");
        videoPlayer.Play();
        print("video is playing: " + videoPlayer.isPlaying);
    }
}

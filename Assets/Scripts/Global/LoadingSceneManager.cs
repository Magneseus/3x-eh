using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour {


    public Image loadingPic;
    public AudioSource audioPlayer;


    private bool isInTrans = false;
    private float transition;
    private bool isShowing = false;
    private float duration;
    private Color colorRecorder;

	// Use this for initialization
	void Start () {
        colorRecorder = loadingPic.color;
        Fade(false, 3f);
        audioPlayer.Play();		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isInTrans) return;
       // transition += isShowing ? Time.deltaTime / duration : -Time.deltaTime / duration;
        transition = Time.deltaTime/duration;
        loadingPic.color = Color.Lerp(loadingPic.color, colorRecorder, transition);
        audioPlayer.volume += isShowing ? -Time.deltaTime / duration : Time.deltaTime / duration;
	}

    public void Fade(bool isShowing, float duration)
    {
        //Debug.Log("Fade is called");
        this.isShowing = isShowing;
        this.duration = duration;
        isInTrans = true;
        transition = isShowing ? 0 : 1;
        colorRecorder.a = isShowing ? 1f : 0;
    }

    public bool getIsInTrans()
    {
        return isInTrans;
    }
}

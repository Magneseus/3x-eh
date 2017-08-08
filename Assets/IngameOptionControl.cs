using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameOptionControl : MonoBehaviour {
    public Text fsSwitchText;

    private GameObject sfxLibrary;
    public GameObject bgmLibrary;
    public AudioSource[] sfxs;
    public AudioSource[] bgms;

    public float masterVolume = 1.00f;
    public float bgmVolume = 1.00f;
    public float sfxVolume = 1.00f;

    public GameObject ingameFullScreenSwitch;
    public GameObject ingameMasterVolume;
    public GameObject ingameSFXVolume;
    public GameObject ingameBGMVolume;
    public GameObject masterVolumeNumber;
    public GameObject bgmVolumeNumber;
    public GameObject sfxVolumeNumber;
    // Use this for initialization
    void Start () {
        sfxLibrary = GameObject.Find("SfxLibrary");
        Debug.Log(sfxLibrary);
        bgmLibrary = GameObject.Find("BGMAudio");
        Debug.Log(bgmLibrary);
        sfxs = sfxLibrary.GetComponents<AudioSource>();
        bgms = bgmLibrary.GetComponents<AudioSource>();
        Debug.Log(bgms);
        masterVolume = AudioListener.volume;

    }
	
	// Update is called once per frame
	void Update () {
        
            ingameFullScreenSwitch.transform.GetChild(0).GetComponent<Text>().text = fsSwitchText.text;
        
      
            ingameMasterVolume = GameObject.Find("MasterSliderInGame");
       
            ingameBGMVolume = GameObject.Find("BGMSliderInGame");
        
            ingameSFXVolume = GameObject.Find("SFXSliderInGame");
        

    }
    public void FullscreenOnClick()
    {
        if (fsSwitchText.text == "ON")
        {
            Screen.fullScreen = false;
            fsSwitchText.text = "OFF";
        }
        else if (fsSwitchText.text == "OFF")
        {
            Screen.fullScreen = true;
            fsSwitchText.text = "ON";
        }
    }

    public void BgmVolumeValueControl()
    {
        bgmVolumeNumber.GetComponent<Text>().text = ((int)(ingameBGMVolume.GetComponent<Slider>().value * 100)).ToString();
        bgmVolume = ingameBGMVolume.GetComponent<Slider>().value;
        foreach (AudioSource bgm in bgms)
        {
            bgm.volume = bgmVolume * masterVolume;
        }

    }

    public void SfxVolumeValueControl()
    {
        sfxVolumeNumber.GetComponent<Text>().text = ((int)(ingameSFXVolume.GetComponent<Slider>().value * 100)).ToString();
        sfxVolume = ingameSFXVolume.GetComponent<Slider>().value;
        foreach (AudioSource sfx in sfxs)
        {
            sfx.volume = sfxVolume * masterVolume;
        }
    }

    public void MasterVolumeValueControl()
    {
        masterVolumeNumber.GetComponent<Text>().text = ((int)(ingameMasterVolume.GetComponent<Slider>().value * 100)).ToString();
        masterVolume = ingameMasterVolume.GetComponent<Slider>().value;
        AudioListener.volume = masterVolume;
    }
}

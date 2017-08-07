using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionControl : MonoBehaviour {

	public Text fsSwitchText;
	public GameObject masterVolumeSlider;
	public GameObject bgmVolumeSlider;
	public GameObject sfxVolumeSlider;
	public GameObject masterVolumeNumber;
	public GameObject bgmVolumeNumber;
	public GameObject sfxVolumeNumber;

	public GameObject sfxLibrary;
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

	// Use this for initialization
	void Start () {
		sfxs = sfxLibrary.GetComponents<AudioSource> ();
		bgms = bgmLibrary.GetComponents<AudioSource> ();
		masterVolume = AudioListener.volume;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("FSswitchInGame") != null){
			ingameFullScreenSwitch = GameObject.Find ("FSswitchInGame");
			ingameFullScreenSwitch.transform.GetChild (0).GetComponent<Text> ().text = fsSwitchText.text;
		}
		if (GameObject.Find ("MasterSliderInGame") != null) {
			ingameMasterVolume = GameObject.Find ("MasterSliderInGame");
		}
		if (GameObject.Find ("BGMSliderInGame") != null) {
			ingameBGMVolume = GameObject.Find ("BGMSliderInGame");
		}
		if (GameObject.Find ("SFXSliderInGame") != null) {
			ingameSFXVolume = GameObject.Find ("SFXSliderInGame");
		}
		
	}

	public void FullscreenOnClick(){
		if (fsSwitchText.text == "ON"){
			Screen.fullScreen = false;
			fsSwitchText.text = "OFF";
		}
		else if (fsSwitchText.text == "OFF"){
			Screen.fullScreen = true;
			fsSwitchText.text = "ON";
		}
	}

	public void BgmVolumeValueControl(){
		bgmVolumeNumber.GetComponent<Text> ().text = ((int)(bgmVolumeSlider.GetComponent<Slider> ().value * 100)).ToString();
		bgmVolume = bgmVolumeSlider.GetComponent<Slider> ().value;
		foreach (AudioSource bgm in bgms) {
			bgm.volume = bgmVolume * masterVolume;
		}
			
	}

	public void SfxVolumeValueControl(){
		sfxVolumeNumber.GetComponent<Text> ().text = ((int)(sfxVolumeSlider.GetComponent<Slider> ().value * 100)).ToString();
		sfxVolume = sfxVolumeSlider.GetComponent<Slider> ().value;
		foreach (AudioSource sfx in sfxs) {
			sfx.volume = sfxVolume * masterVolume;
		}
	}

	public void MasterVolumeValueControl(){
		masterVolumeNumber.GetComponent<Text> ().text = ((int)(masterVolumeSlider.GetComponent<Slider> ().value * 100)).ToString();
		masterVolume = masterVolumeSlider.GetComponent<Slider> ().value;
		AudioListener.volume = masterVolume;
	}
}

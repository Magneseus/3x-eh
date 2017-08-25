using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	public GameObject bgmLibrary;
	public GameObject sfxLibrary;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)){
			sfxLibrary.GetComponents<AudioSource>()[0].Play();
		}
        if (GameObject.Find("City(Clone)") != null)
        {
            if (GameObject.FindObjectOfType<GameController>().dGame.currentCity.Name == "ottawa")
            {
                if (GameObject.Find("BGMAudio") != null)
                {
                    GameObject.Find("BGMAudio").GetComponent<AudioSource>().clip = bgmLibrary.GetComponents<AudioSource>()[0].clip;
                    //GameObject.Find ("BGMAudio").GetComponent<AudioSource> ().Play ();
                }
            }
            if (GameObject.FindObjectOfType<GameController>().dGame.currentCity.Name == "iqaluit")
            {
                if (GameObject.Find("BGMAudio") != null)
                {
                    GameObject.Find("BGMAudio").GetComponent<AudioSource>().clip = bgmLibrary.GetComponents<AudioSource>()[1].clip;
                    //GameObject.Find ("BGMAudio").GetComponent<AudioSource> ().Play ();
                }
            }
            if (GameObject.FindObjectOfType<GameController>().dGame.currentCity.Name == "vancouver")
            {
                if (GameObject.Find("BGMAudio") != null)
                {
                    GameObject.Find("BGMAudio").GetComponent<AudioSource>().clip = bgmLibrary.GetComponents<AudioSource>()[2].clip;
                    //GameObject.Find ("BGMAudio").GetComponent<AudioSource> ().Play ();
                }
            }
        }
    }
}
